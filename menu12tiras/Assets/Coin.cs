using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float scale = 0.1f;
    public float yratio = 0.07f;

    private void OnTriggerEnter(Collider other)
    {
        // check if player touches coin
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // score


        // destroy coin
        Destroy(gameObject);
    }

    private void Start()
    {
        transform.localScale = new Vector3(scale, scale * yratio, scale);
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
