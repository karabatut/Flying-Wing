using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Part
{
    public List<Vector3> vertices { get; set; }
    public List<int> triangles { get; set; }
    public List<Vector2> uvs { get; set; }
    public int vertexIndex { get; set; }
    public Mesh mesh { get; set; }
    public List<Vector3> sideSnapPoints { get; set; }
    public List<Vector3> frontSnapPoints { get; set; }
    public GameObject partObject { get; set; }
    public Dictionary<Vector3, Vector3> sideSnapPointsWNormals { get; set; }
    public SnappingEnum isSideSnapped { get; set; }

    public Triangle()
    {
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uvs = new List<Vector2>();
        this.vertexIndex = 0;
        this.mesh = new Mesh();
        this.sideSnapPoints = new List<Vector3>();
        this.frontSnapPoints = new List<Vector3>();
    }

    MeshCreator meshCreator = new MeshCreator();

    public Part CreatePart(int[] baseStartMargins, int[] tipPositionXMargins, int[] tipPositionYMargins, int snapPoints)
    {
        Vector3 baseStart;
        Vector3 baseEnd;
        Vector3 tip;

        int length;

        Part part;

        int baseStartYValue = Random.Range(baseStartMargins[0], baseStartMargins[1]); //3,20
        int baseEndYValue = 0;
        int tipYValue = Random.Range(tipPositionXMargins[0], tipPositionXMargins[1]); //0,30
        int tipXValue = Random.Range(tipPositionYMargins[0], tipPositionYMargins[1]); //3, 20

        baseStart = new Vector3(0f, baseStartYValue, 0f);
        baseEnd = new Vector3(0f, baseEndYValue, 0f);
        tip = new Vector3(tipXValue, tipYValue, 0f);

        length = baseStartYValue - baseEndYValue;


        Bresenham3D TopToBottom = new Bresenham3D(baseStart, baseEnd);
        Bresenham3D BottomToTip = new Bresenham3D(baseEnd, tip);
        Bresenham3D TopToTip = new Bresenham3D(baseStart, tip);

        this.frontSnapPoints.Add(baseStart);
        this.frontSnapPoints.Add(baseEnd);

        int positionIndex = 0;
        foreach (Vector3 position in TopToBottom)
        {
            part = meshCreator.AddVoxelToChunk(position, this);

            if(positionIndex == length / 2)
            {
                this.sideSnapPoints.Add(position);
            }
            positionIndex++;
        }
        this.sideSnapPoints.Add(tip);

        foreach (Vector3 topPosition in TopToTip)
        {
            foreach (Vector3 bottomPosition in BottomToTip)
            {
                if(topPosition.x == bottomPosition.x)
                {
                    Vector3 position = topPosition;
                    while (position.y >= bottomPosition.y)
                    {
                        part = meshCreator.AddVoxelToChunk(position, this);
                        position.y--;
                    }
                }
            }
        }
        foreach (Vector3 position in BottomToTip)
        {
            part = meshCreator.AddVoxelToChunk(position, this);
        }

        return meshCreator.CreateMesh(this);
    }


}
