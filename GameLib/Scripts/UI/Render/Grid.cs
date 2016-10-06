using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    public int XSize, YSize;

    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Map";

        vertices = new Vector3[(XSize + 1) * (YSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0, y = 0; y <= YSize; y++)
        {
            for (int x = 0; x <= XSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / XSize, (float)y / YSize);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;

        int[] triangles = new int[XSize * YSize * 6];
        for (int ti = 0, vi = 0, y = 0; y < YSize; y++, vi++)
        {
            for (int x = 0; x < XSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + XSize + 1;
                triangles[ti + 5] = vi + XSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

