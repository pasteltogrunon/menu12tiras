using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{

    //[Range(0, 2 * Mathf.PI)] public float u = 0f;
    //[Range(-1, 1)] public float v = 0f;
    [Range(0, 20)] public float radius = 1f;
    [Range(0, 20)] public float width = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float u = 0;
        float v = -1;
        float step = 0.01f;
        while (v < 1)
        {
            while (u < 2 * Mathf.PI)
            {
                Gizmos.DrawLine(Mobius(u, v, radius, width), Mobius(u + step, v, radius, width));
                u += step;
            }
            u = 0;
            v += step;
        }
    }

    private Vector3 Mobius(float u, float v, float radius = 1f, float width = 0.5f)
    {
        float x = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Cos(u);
        float z = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Sin(u);
        float y = v * Mathf.Sin(u / 2f);

        return new Vector3(x, y, z);
    }

}
