
using System.Collections.Generic;
using UnityEngine;

public interface Part
{
    List<Vector3> vertices { get; set; }
    List<int> triangles { get; set; }
    List<Vector2> uvs { get; set; }
    int vertexIndex { get; set; }
    Mesh mesh { get; set; }
    List<Color> vertexColors { get; set; }

    GameObject partObject { get; set; }

    List<Vector3> sideSnapPoints { get; set; }
    List<Vector3> frontSnapPoints { get; set; }
    SnappingEnum isSideSnapped { get; set; }
    Dictionary<Vector3, Vector3> sideSnapPointsWNormals { get; set; }

    Part CreatePart(int[] firstParam, int[] secondParam, int[] thirdParam, int fourthParam, ColorParameters colorParameter);
}
