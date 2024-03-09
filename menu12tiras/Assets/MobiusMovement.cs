using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobiusMovement : MonoBehaviour
{
    [Range(0, 20)] public float radius = 1f;
    [Range(0, 20)] public float width = 0.5f;
    [Range(0, 10)] public float speed = 1f;

    private Vector3 offset = new Vector3(0, 0, -0.1f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Mobius(Time.time * speed, 0, radius, width);
    }

    private Vector3 Mobius(float u, float v, float radius = 1f, float width = 0.5f)
    {
        float x = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Cos(u);
        float z = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Sin(u);
        float y = v * Mathf.Sin(u / 2f);

        return new Vector3(x, y, z) + offset;
    }

}
