using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : Part
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

    public Sphere()
    {
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uvs = new List<Vector2>();
        this.vertexIndex = 0;
        this.mesh = new Mesh();
        this.sideSnapPoints = new List<Vector3>();
        this.frontSnapPoints = new List<Vector3>();
        this.sideSnapPointsWNormals = new Dictionary<Vector3, Vector3>();
    }

    MeshCreator meshCreator = new MeshCreator();

    public Part CreatePart(int[] radiusMargins, int[] centerDistances, int[] thirdParam, int fourthParam)
    {
        int radius;
        int centerDistance;


        Part part;
        List<Vector3> corners = new List<Vector3>();

        radius = Random.Range(radiusMargins[0], radiusMargins[1]);
        centerDistance = Random.Range(centerDistances[0], centerDistances[1]);
        Vector3 firstCenter = new Vector3(centerDistance / 2, 0f, 0f);
        Vector3 secondCenter = new Vector3(-centerDistance / 2, 0f, 0f);
        
        int a = radius + centerDistance / 2;
        int b = (int)Mathf.Sqrt(a * a - radius * radius);
        if(b == 0)
        {
            b = radius;
        }

        for (int x = -a; x < a; x++)
        {
            for (int y = -b; y < b; y++)
            {
                for (int z = -b; z < b; z++)
                {
                    Vector3 voxel = new Vector3(x, y, z);
                    float temp = Vector3.Distance(voxel, firstCenter) + Vector3.Distance(voxel, secondCenter);
                    if ((temp < (2 * a)) && (temp > (b - 1.5f)))
                    {
                        corners.Add(voxel);
                    }
                }
            }
        }           

        this.sideSnapPoints.Add(new Vector3(a, 0, 0));
        this.sideSnapPoints.Add(new Vector3(-a, 0, 0));
        this.sideSnapPoints.Add(new Vector3(0, 0, b));
        this.sideSnapPoints.Add(new Vector3(0, 0, -b));

        this.frontSnapPoints.Add(new Vector3(0, radius, 0));
        this.frontSnapPoints.Add(new Vector3(0, -radius, 0));
        
        foreach (Vector3 position in corners)
        {
            part = meshCreator.AddVoxelToChunk(position, this);
            this.vertices = part.vertices;
            this.triangles = part.triangles;
            this.uvs = part.uvs;
            this.vertexIndex = part.vertexIndex;
            this.vertices = part.vertices;
        }
        return meshCreator.CreateMesh(this);
    }
}
