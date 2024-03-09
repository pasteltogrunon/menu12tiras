using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobiusStripTwisted
{

    private const float EPSILON = 0.01f;

    [Header("Mobius Strip Structure")]
    [Range(0, 80)] public float radius;
    [Range(0, 40)] public float width;
    [Range(0, 20)] public float separation;
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

    public Vector2 F(float t)
    {
        float x = radius * Mathf.Cos(t) / (1 + Mathf.Sin(t) * Mathf.Sin(t));
        float y = radius * Mathf.Cos(t) * Mathf.Sin(t) / (1 + Mathf.Sin(t) * Mathf.Sin(t));
        return new Vector2(x, y);
    }

    public Vector2 G(float t)
    {
        float DF_x = -radius * Mathf.Sin(t) * (Mathf.Sin(t) * Mathf.Sin(t) + 2 * Mathf.Cos(t) * Mathf.Cos(t) + 1) / Mathf.Pow(1 + Mathf.Sin(t) * Mathf.Sin(t), 2);
        float DF_y = -radius * (Mathf.Pow(Mathf.Sin(t), 4) + (1 + Mathf.Cos(t) * Mathf.Cos(t)) * Mathf.Sin(t) * Mathf.Sin(t) - Mathf.Cos(t) * Mathf.Cos(t)) / Mathf.Pow(1 + Mathf.Sin(t) * Mathf.Sin(t), 2);

        float norma = Mathf.Sqrt(DF_x * DF_x + DF_y * DF_y);

        float x = DF_x / norma;
        float y = DF_y / norma;

        return new Vector2(y, -x);
    }

    public Vector3 GetPosition(float u, float v)
    {
        v = Mathf.Clamp(v, -0.5f + EPSILON, 0.5f - EPSILON);
        Vector2 FValue = F(u);
        Vector2 GValue = G(u);
        float x = FValue.x + v * width * Mathf.Cos(u / 2 - Mathf.PI / 4) * GValue.x;
        float y = FValue.y + v * width * Mathf.Cos(u / 2 - Mathf.PI / 4) * GValue.y;
        float z = v * width * Mathf.Sin(u / 2 - Mathf.PI / 4) - separation * Mathf.Sin(u) / 2;

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
