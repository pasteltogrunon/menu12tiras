using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float scale = 0.1f;
    public float yratio = 0.07f;

    private float u, v;
    private bool inverted;

    public Material activeCoinMaterial;
    public Material inactiveCoinMaterial;

    public AudioSource source;
    public ParticleSystem FX;

    public float U { get; set; }
    public float V { get; set; }
    public bool Inverted
    {
        get => inverted;
        set => inverted = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if player touches coin
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // score
        GameObject.Find("Player").GetComponent<Player>().Coins++;

        source.Play();
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetComponent<Collider>().enabled = false;
        FX.Play();
        FX.transform.SetParent(null);
        Destroy(FX.gameObject, 4f);
        Destroy(gameObject, 4f);
    }

    private void Start()
    {
        transform.localScale = new Vector3(scale, scale * yratio, scale);
    }

    private void Update()
    {
        transform.GetChild(0).Rotate(GameManager.instance.GetMobiusStripNormal(u, v), rotationSpeed * Time.deltaTime);
        if (inverted == GameObject.Find("Player").GetComponent<Player>().Inverted)
            transform.GetChild(0).GetComponent<MeshRenderer>().material = activeCoinMaterial;
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = inactiveCoinMaterial;
        }
    }

    public void Init(float u, float v, bool inverted = false)
    {
        name = $"Coin_{u}_{v}";
        this.u = u;
        this.v = v;
        this.inverted = inverted;
        Vector3 position = GameManager.instance.GetMobiusStripPosition(u, v);
        Vector3 lookAt = GameManager.instance.GetMobiusStripLookAt(u, v);
        Vector3 normal = GameManager.instance.GetMobiusStripNormal(u, v);
        transform.SetPositionAndRotation(position + (inverted ? -1 : 1) * 0.2f * normal, Quaternion.LookRotation(lookAt, normal));
    }
}
