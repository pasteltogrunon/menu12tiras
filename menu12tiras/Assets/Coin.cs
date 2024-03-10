using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 90f;

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

    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
