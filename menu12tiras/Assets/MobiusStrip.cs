using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobiusStrip
{

    private const float EPSILON = 0.01f;

    [Header("Mobius Strip Structure")]
    [Range(0, 20)] public float radius;
    [Range(0, 20)] public float width;
    public Matrix4x4 axis;

    [Header("Mobius Strip Mesh")]
    public Material material;
    public int uResolution;
    public int vResolution;


    private GameObject strip;
    private Mesh mesh;

    public void GenerateMobiusStrip(Material material, int uResolution, int vResolution)
    {
        strip = new GameObject("Mobius Strip");
        strip.AddComponent<MeshFilter>();
        strip.AddComponent<MeshRenderer>();
        strip.transform.position = Vector3.zero;
        mesh = new Mesh();
        strip.GetComponent<MeshFilter>().mesh = mesh;
        strip.GetComponent<MeshRenderer>().material = material;
        strip.GetComponent<MeshRenderer>().material.color = Color.white;

        List<Vector3> vertices = new();
        List<Vector2> uv = new();
        List<int> triangles = new();

        float ustep = 2 * Mathf.PI / uResolution;
        float vstep = 2f / vResolution;

        float u = 0;
        float v = -1;

        while (v < 1)
        {
            while (u < 2 * Mathf.PI)
            {
                Vector3 p0 = GetPosition(u, v);
                Vector3 p1 = GetPosition(u + ustep, v);
                Vector3 p2 = GetPosition(u + ustep, v + vstep);
                Vector3 p3 = GetPosition(u, v + vstep);

                vertices.Add(p0);
                vertices.Add(p1);
                vertices.Add(p2);
                vertices.Add(p3);

                uv.Add(new Vector2(u / (2 * Mathf.PI), (v + 1) / 2));
                uv.Add(new Vector2((u + ustep) / (2 * Mathf.PI), (v + 1) / 2));
                uv.Add(new Vector2((u + ustep) / (2 * Mathf.PI), (v + vstep + 1) / 2));
                uv.Add(new Vector2(u / (2 * Mathf.PI), (v + vstep + 1) / 2));

                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 1);

                u += ustep;
            }
            u = 0;
            v += vstep;
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    public Vector3 GetPosition(float u, float v)
    {
        v = Mathf.Clamp(v, -1 + EPSILON, 1 - EPSILON);
        float x = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Cos(u);
        float y = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Sin(u);
        float z = v * Mathf.Sin(u / 2f);

        return axis.MultiplyPoint(new Vector3(x, y, z));
    }

    public Vector3 Normal(float u, float v)
    {
        Vector3 p0 = GetPosition(u, v);
        Vector3 p1 = GetPosition(u + EPSILON, v);
        Vector3 p2 = GetPosition(u, v + EPSILON);
        return Vector3.Cross(p1 - p0, p2 - p0).normalized;
    }

    public Vector3 LookAt(float u, float v)
    {
        Vector3 p0 = GetPosition(u, v);
        Vector3 p1 = GetPosition(u + EPSILON, v);
        return (p1 - p0).normalized;
    }

    public GameObject GetMobiusStrip()
    {
        return strip;
    }
}
