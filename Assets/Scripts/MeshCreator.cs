using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator 
{

    public Part AddVoxelToChunk(Vector3 position, Part part)
    {
        List<Vector3> vertices = part.vertices;
        List<int> triangles = part.triangles;
        List<Vector2> uvs = part.uvs;
        int vertexIndex = part.vertexIndex;

        List<Vector3> verticesTemp = new List<Vector3>();

        for (int k = 0; k < 6; k++)
        {
            for (int i = 0; i < 6; i++)
            {
                int triangleIndex = VoxelData.voxelTris[k, i];



                //vertices.Add(VoxelData.voxelVerts[triangleIndex] + position);

                if(!verticesTemp.Contains(VoxelData.voxelVerts[triangleIndex] + position))
                {
                    verticesTemp.Add(VoxelData.voxelVerts[triangleIndex] + position);
                    uvs.Add(Vector2.zero);
                }
                
                if(i == 3)
                {
                    triangles.Add(triangles[triangles.Count - 1]);
                }
                else if(i == 4)
                {
                    triangles.Add(triangles[triangles.Count - 3]);
                }
                else
                {
                    triangles.Add(vertexIndex);
                    vertexIndex++;
                }


                
            }
            vertices.AddRange(verticesTemp);
            verticesTemp.Clear();
        }



        part.vertices = vertices;
        part.triangles = triangles;
        part.uvs = uvs;
        part.vertexIndex = vertexIndex;



       /* for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                int index = VoxelData.triangles[6*i+k];
                Vector3 vector = VoxelData.vertices[index] + position;
                if (!verticesTemp.Contains(vector))
                {
                    verticesTemp.Add(vector);
                }
                
                triangles.Add(index + vertexIndex * 8);
            }
            vertices.AddRange(verticesTemp);
        }*/

        /*foreach (int vertex in VoxelData.triangles)
        {
            Vector3 vector = VoxelData.vertices[vertex] + position;
            vertices.Add(vector);
            triangles.Add(vertex + vertexIndex * 8);
        }*/

        /*part.vertices = vertices;
        part.triangles = triangles;
        vertexIndex++;
        part.vertexIndex = vertexIndex;*/

        /*foreach (Vector3 item in VoxelData.vertices)
        {
            Vector3 vector = item + position;
            vertices.Add(vector);
            uvs.Add(new Vector2(1, 1));
        }

        part.vertices = vertices;
        foreach (int item in VoxelData.triangles)
        {
            triangles.Add(item + vertexIndex * 8);
        }

        part.triangles = triangles;
        part.uvs = uvs;

        vertexIndex++;
        part.vertexIndex = vertexIndex;*/

        return part;
    }

    public Part CreateMesh(Part part)
    {
        Mesh mesh = part.mesh;

        if(part.vertices.Count > 65535)
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        mesh.vertices = part.vertices.ToArray();
        mesh.triangles = part.triangles.ToArray();
        mesh.uv = part.uvs.ToArray();

        mesh.RecalculateNormals();
        part.mesh = mesh;

        return part;
    }
}
