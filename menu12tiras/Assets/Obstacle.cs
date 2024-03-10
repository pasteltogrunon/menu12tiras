using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float speedMultiplier = 0.5f;
    public float hoffset = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<Player>().USpeed *= speedMultiplier;
        }

        Destroy(gameObject);
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



    public void Init(float u, float v, bool inverted = false)
    {
        name = $"Obstacle_{u}_{v}";
        this.u = u;
        this.v = v;
        Vector3 position = GameManager.instance.GetMobiusStripPosition(u, v);
        Vector3 lookAt = GameManager.instance.GetMobiusStripLookAt(u, v);
        Vector3 normal = GameManager.instance.GetMobiusStripNormal(u, v);
        transform.SetPositionAndRotation(position + (inverted ? -1 : 1) * hoffset * normal, Quaternion.LookRotation(lookAt, normal));
    }
}
