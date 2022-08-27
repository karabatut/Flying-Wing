using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : Part
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
    public List<Color> vertexColors { get; set; }


    public Rectangle()
    {
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uvs = new List<Vector2>();
        this.vertexIndex = 0;
        this.mesh = new Mesh();
        this.sideSnapPoints = new List<Vector3>();
        this.frontSnapPoints = new List<Vector3>();
        this.vertexColors = new List<Color>();
    }
    MeshCreator meshCreator = new MeshCreator();

    public Part CreatePart(int[] lengthMargins, int[] widthMargins, int[] heightMargins, int fourthParam, ColorParameters colorParameters)
    {
        int length;
        int width;
        int height;


        Part part;
        List<Vector3> corners = new List<Vector3>();

        length = Random.Range(lengthMargins[0], lengthMargins[1]);
        width = Random.Range(widthMargins[0], widthMargins[1]);
        height = Random.Range(heightMargins[0], heightMargins[1]);

        for (int i = -width/2; i < width/2; i++)//x
        {
            for (int j = -height/2; j < height/2; j++)//y
            {
                for (int k = -length/2; k < length/2; k++)//z
                {
                    Vector3 position = new Vector3(i, j, k);
                    corners.Add(position);
                   
                }
                
            }
        }

        this.sideSnapPoints.Add(new Vector3(width /2, 0, 0));
        this.sideSnapPoints.Add(new Vector3(-width /2, 0, 0));
        this.sideSnapPoints.Add(new Vector3(0, 0, length/2));
        this.sideSnapPoints.Add(new Vector3(0, 0, -length / 2));
        
        this.frontSnapPoints.Add(new Vector3(0, height / 2, 0));
        this.frontSnapPoints.Add(new Vector3(0, -height / 2, 0));

        foreach (Vector3 position in corners)
        {
            part = meshCreator.AddVoxelToChunk(position, this);
            this.vertices = part.vertices;
            this.triangles = part.triangles;
            this.uvs = part.uvs;
            this.vertexIndex = part.vertexIndex;
            this.vertices = part.vertices;
        }
        return meshCreator.CreateMesh(this, colorParameters);
    }
}
