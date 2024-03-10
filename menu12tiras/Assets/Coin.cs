using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float scale = 0.1f;
    public float yratio = 0.07f;

    private float u, v;

    public float U { get; set; }
    public float V { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        // check if player touches coin
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // score
        GameObject.Find("Player").GetComponent<Player>().Coins++;

        // destroy coin
        Destroy(gameObject);
    }

    private void Start()
    {
        transform.localScale = new Vector3(scale, scale * yratio, scale);
    }

    private void Update()
    {
        transform.Rotate(GameManager.instance.GetMobiusStripNormal(u, v), rotationSpeed * Time.deltaTime);
    }

    public void Init(float u, float v, bool inverted = false)
    {
        name = $"Coin_{u}_{v}";
        this.u = u;
        this.v = v;
        Vector3 position = GameManager.instance.GetMobiusStripPosition(u, v);
        Vector3 lookAt = GameManager.instance.GetMobiusStripLookAt(u, v);
        Vector3 normal = GameManager.instance.GetMobiusStripNormal(u, v);
        transform.SetPositionAndRotation(position + (inverted ? -1 : 1) * 0.2f * normal, Quaternion.LookRotation(lookAt, normal));
    }
}
