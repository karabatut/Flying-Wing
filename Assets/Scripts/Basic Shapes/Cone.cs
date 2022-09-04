using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : Part
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

    public Cone()
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

    public Part CreatePart(int[] lengthMargins, int[] subdivisionMargins, int[] baseRadiusMargins, int snapPoints, ColorParameters colorParameters)
    {
        int length;
        int baseRadius;

        int previousRadius = 0;

        Part part;

        length = Random.Range(lengthMargins[0], lengthMargins[1]); 
        baseRadius = Random.Range(baseRadiusMargins[0], baseRadiusMargins[1]); 

        List<Vector3> corners = new List<Vector3>();

        Vector3 tipPoint = new Vector3(0f, length/2, 0f);

        Vector3 basePoint = new Vector3(tipPoint.x + baseRadius, tipPoint.y - length, 0f);

        Bresenham3D line = new Bresenham3D(tipPoint, basePoint);

        float middlePoint = 0;

        foreach (Vector3 position in line)
        {
            int tempRadius = (int)position.x;
            
            for (int i = -tempRadius; i < tempRadius; i++)
            {
                for (int j = -tempRadius; j < tempRadius; j++)
                {
                    Vector3 origin = new Vector3(0f, position.y, 0);
                    Vector3 temp = new Vector3(i, position.y, j);
                    if ((Vector3.Distance(origin, temp) <= tempRadius) && (Vector3.Distance(origin, temp) >= (previousRadius-1.5f)))
                    {
                        corners.Add(temp);
                        if(position.y == 0)
                        {
                            middlePoint = temp.x;
                        }
                    }
                }
                
            }
            previousRadius = tempRadius;

           
        }


        sideSnapPoints.Add(new Vector3(middlePoint, length / 2, 0));
        sideSnapPoints.Add(new Vector3(-middlePoint, length / 2, 0));
        sideSnapPoints.Add(new Vector3(0, length / 2, middlePoint));
        sideSnapPoints.Add(new Vector3(0, length / 2, -middlePoint));

        this.frontSnapPoints.Add(tipPoint);
        Vector3 tempSnap = basePoint;
        tempSnap.x = 0;
        this.frontSnapPoints.Add(tempSnap);

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

