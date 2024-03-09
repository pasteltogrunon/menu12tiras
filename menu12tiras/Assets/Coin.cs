using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
