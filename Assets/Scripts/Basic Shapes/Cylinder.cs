using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : Part
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
    public int radius { get; set; }
    public SnappingEnum isSideSnapped { get; set; }
    public List<Color> vertexColors { get; set; }

    public Cylinder()
    {
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uvs = new List<Vector2>();
        this.vertexIndex = 0;
        this.mesh = new Mesh();
        this.sideSnapPoints = new List<Vector3>();
        this.frontSnapPoints = new List<Vector3>();
        this.sideSnapPointsWNormals = new Dictionary<Vector3, Vector3>();
        this.vertexColors = new List<Color>();
    }

    MeshCreator meshCreator = new MeshCreator();

    public Part CreatePart(int[] lengthMargins, int[] subdivisionMargins, int[] radiusMargins, int snapPoints, ColorParameters colorParameters)
    {
        int length;
        int sides;
        int radius;

        int sideLength = 0;

        Part part;


        length = Random.Range(lengthMargins[0], lengthMargins[1]); //5,30
        sides = Random.Range(subdivisionMargins[0], subdivisionMargins[1]); //3,10
        radius = Random.Range(radiusMargins[0], radiusMargins[1]); //5,12

        this.radius = radius;


        Vector3[] corners = new Vector3[sides];

        float angle = 0f;
        for (int i = 0; i < sides; i++)
        {
            angle = 360/sides * i;
            int x = (int)(radius * Mathf.Cos(angle * Mathf.Deg2Rad));
            int z = (int)(radius * Mathf.Sin(angle * Mathf.Deg2Rad));

            Vector3 temp = new Vector3(x, -length/2, z);
            corners[i] = temp;
        }

        for (int i = 0; i < corners.Length; i++)
        {
            Bresenham3D line;
            if (i == corners.Length - 1)
            {
                line = new Bresenham3D(corners[i], corners[0]);
            }
            else
            {
                line = new Bresenham3D(corners[i], corners[i + 1]);
            }

            foreach (Vector3 position in line)
            {
                sideLength++;
            }

            int count = 0;
            
            foreach (Vector3 position in line)
            {
                Vector3 temp = position;
                for (int j = -length/2; j < length/2; j++)
                {
                    part = meshCreator.AddVoxelToChunk(temp, this);
                    this.vertices = part.vertices;
                    this.triangles = part.triangles;
                    this.uvs = part.uvs;
                    this.vertexIndex = part.vertexIndex;
                    this.vertices = part.vertices;
                    temp.y++;

                    if ((j == 0))
                    {
                        if(count == sideLength / 2)
                        {
                            this.sideSnapPoints.Add(temp);
                        }
                        
                    }
                    
                }

                count++;
            }
            sideLength = 0;
            count = 0;

        }

        
        this.frontSnapPoints.Add(new Vector3(0f, length/2, 0f));
        this.frontSnapPoints.Add(new Vector3(0f, -length / 2, 0f));

        return meshCreator.CreateMesh(this, colorParameters);
    }
}
