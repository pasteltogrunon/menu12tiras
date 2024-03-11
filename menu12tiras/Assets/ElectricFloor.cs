using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricFloor : MonoBehaviour
{

    public float speedMultiplier = 0.5f;
    public float hoffset = 0.05f;
    public int uResolution = 10;
    public int vResolution = 3;
    public Material material;
    public AudioSource source;
    [SerializeField] private ParticleSystem FX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<Player>().USpeed = Mathf.Clamp(other.GetComponent<Player>().USpeed * speedMultiplier, 2f, 4f);
        }

        source.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;
        FX.Play();
        FX.transform.SetParent(null);
        Destroy(FX.gameObject, 4f);
        Destroy(gameObject, 4f);
    }

    private float u, v;
    private bool inverted;

    public float U { get; set; }
    public float V { get; set; }
    public bool Inverted
    {
        get => inverted;
        set => inverted = value;
    }

    public void Init(float u, float v, float ulength, float vlength, bool inverted = false)
    {
        Debug.Log($"ElectricFloor.Init - u: {u}, v: {v}, ulength: {ulength}, vlength: {vlength}, inverted: {inverted}");
        name = $"Obstacle_{u}_{v}";
        this.u = u;
        this.v = v;
        this.inverted = inverted;
        GenerateElectricField(ulength, vlength);
    }

    private void GenerateElectricField(float ulength, float vlength)
    {
        name = $"ElectricField_{u}_{v}";
        Mesh mesh = new();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        transform.position = Vector3.zero + (inverted ? -1 : 1) * hoffset * GameManager.instance.GetMobiusStripNormal(u, v);
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;

        List<Vector3> vertices = new();
        List<Vector2> uv = new();
        List<int> triangles = new();

        float ustep = ulength / (float)uResolution;
        float vstep = vlength / (float)vResolution;

        float mu = u;
        float mv = v;

        while (mv < v + vlength)
        {
            while (mu < u + ulength)
            {
                Vector3 p0 = GameManager.instance.GetMobiusStripPosition(mu, Mathf.Clamp(mv, v + GameManager.EPSILON, v + vlength - GameManager.EPSILON));
                Vector3 p1 = GameManager.instance.GetMobiusStripPosition(mu + ustep, Mathf.Clamp(mv, v + GameManager.EPSILON, v + vlength - GameManager.EPSILON));
                Vector3 p2 = GameManager.instance.GetMobiusStripPosition(mu + ustep, Mathf.Clamp(mv + vstep, v + GameManager.EPSILON, v + vlength - GameManager.EPSILON));
                Vector3 p3 = GameManager.instance.GetMobiusStripPosition(mu, Mathf.Clamp(mv + vstep, v + GameManager.EPSILON, v + vlength - GameManager.EPSILON));

                vertices.Add(p0);
                vertices.Add(p1);
                vertices.Add(p2);
                vertices.Add(p3);

                uv.Add(new Vector2((mu - u) / ulength, (mv - v) / vlength));
                uv.Add(new Vector2((mu + ustep - u) / ulength, (mv - v) / vlength));
                uv.Add(new Vector2((mu + ustep - u) / ulength, (mv + vstep - v) / vlength));
                uv.Add(new Vector2((mu - u) / ulength, (mv + vstep - v) / vlength));

                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 1);

                mu += ustep;
            }
            mu = u;
            mv += vstep;
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();

        this.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.GetComponent<MeshCollider>().convex = true;
        gameObject.GetComponent<MeshCollider>().isTrigger = true;

    }
}
