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
        List<Color> vertexColors = part.vertexColors;

        List<Vector3> verticesTemp = new List<Vector3>();
        

        for (int k = 0; k < 6; k++)
        {
            for (int i = 0; i < 6; i++)
            {
                int triangleIndex = VoxelData.voxelTris[k, i];



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
        


        return part;
    }

    public Part CreateMesh(Part part, ColorParameters colorParameters)
    {
        Mesh mesh = part.mesh;

        if(part.vertices.Count > 65535)
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }
        List<Color> vertexColorsTemp = new List<Color>();

        int primaryProb = colorParameters.primaryColorProbability;
        int secondaryProb = primaryProb + colorParameters.secondaryColorProbability;
        int tertiaryProb = secondaryProb + colorParameters.tertiaryColorProbability;
        int quternaryProb = tertiaryProb + colorParameters.quternaryColorProbability;

        

        for (int i = 0; i < part.vertices.Count; i = i+24)
        {
            int randomNum = Random.Range(0, quternaryProb);
            if (randomNum <= primaryProb)
            {
                for (int j = 0; j < 24; j++)
                {
                    vertexColorsTemp.Add(colorParameters.primaryColor);
                }
                
            }
            else if(randomNum > primaryProb && randomNum <= secondaryProb)
            {
                for (int j = 0; j < 24; j++)
                {
                    vertexColorsTemp.Add(colorParameters.secondaryColor);
                }
            }
            else if(randomNum > secondaryProb && randomNum <= tertiaryProb)
            {
                for (int j = 0; j < 24; j++)
                {
                    vertexColorsTemp.Add(colorParameters.tertiaryColor);
                }
            }
            else
            {
                for (int j = 0; j < 24; j++)
                {
                    vertexColorsTemp.Add(colorParameters.quternaryColor);
                }
            }
        }

        part.vertexColors = vertexColorsTemp;

        mesh.vertices = part.vertices.ToArray();
        mesh.triangles = part.triangles.ToArray();
        mesh.uv = part.uvs.ToArray();

        mesh.colors = part.vertexColors.ToArray();
        mesh.RecalculateNormals();
        
        part.mesh = mesh;

        return part;
    }

   
}
