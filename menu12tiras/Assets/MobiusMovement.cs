using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class MobiusMovement : MonoBehaviour
{
    [Range(0, 20)] public float radius = 1f;
    [Range(0, 20)] public float width = 0.5f;
    [Range(0, 10)] public float fspeed = 1f;
    [Range(0, 10)] public float hspeed = 1f;

    private float u = 0;
    private float v = 0;
    private float boundEpsilon = 0.01f;
    private int foo = 1;
    Vector3 normal;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foo *= -1;
        }
        v -= Input.GetAxis("Horizontal") * Time.deltaTime * hspeed;
        v = Mathf.Clamp(v, -1 + boundEpsilon, 1 - boundEpsilon);
        Vector3 currentPoint = Mobius(u, Mathf.Sin(v), radius, width);
        Vector3 stepPoint = Mobius(u + 0.01f, Mathf.Sin(v), radius, width);
        Vector3 tangent1 = Mobius(u, Mathf.Sin(v + 0.01f), radius, width) - currentPoint;
        Vector3 tangent2 = stepPoint - currentPoint;
        normal = Vector3.Cross(tangent1, tangent2);
        Vector3 offset = normal.normalized * 0.1f * foo;
        transform.position = currentPoint + offset;
        transform.rotation = Quaternion.LookRotation(tangent2, normal);
        u += Time.deltaTime * fspeed;
    }

    private Vector3 Mobius(float u, float v, float radius = 1f, float width = 0.5f)
    {
        float z = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Cos(u);
        float x = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Sin(u);
        float y = v * Mathf.Sin(u / 2f);

        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -normal * 10);
        for (float i = 0; i < 2 * Mathf.PI; i += 0.01f)
        {
            for (float j = -1; j < 1; j += 0.1f)
            {
                Vector3 p1 = Mobius(i, j, radius, width);
                Gizmos.DrawSphere(p1, 0.01f);
            }
        }
    }

}
