using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus : Part
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


    public Torus()
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

    public Part CreatePart(int[] outerRadiusMargins, int[] innerRadiusMargins, int[] thirdParam, int fourthParam)
    {

        int innerRadius;
        int outerRadius;

        Part part;
        List<Vector3> corners = new List<Vector3>();

        innerRadius = Random.Range(innerRadiusMargins[0], innerRadiusMargins[1]);
        outerRadius = Random.Range(outerRadiusMargins[0], outerRadiusMargins[1]);

        int totalRadius = innerRadius + 2 * outerRadius;


        for (int x = -totalRadius; x < totalRadius; x++)
        {
            for (int z = -totalRadius; z < totalRadius; z++)
            {
                for (int y = -outerRadius; y < outerRadius; y++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    Vector3 normal = position.normalized;
                    normal.y = 0;
                    Vector3 centerLine = normal * (innerRadius + outerRadius);

                    if((Vector3.Distance(position, centerLine) <= innerRadius))
                    {
                        corners.Add(position);
                    }

                }
            }
        }

        this.sideSnapPoints.Add(new Vector3(totalRadius / 2, 0, 0));
        this.sideSnapPoints.Add(new Vector3(0, 0, totalRadius / 2));
        this.sideSnapPoints.Add(new Vector3(-totalRadius / 2, 0, 0));
        this.sideSnapPoints.Add(new Vector3(0, 0, -totalRadius / 2));

        this.frontSnapPoints.Add(new Vector3(0, outerRadius / 2, 0));
        this.frontSnapPoints.Add(new Vector3(0, -outerRadius / 2, 0));


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
