using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobiusStripDefault : MobiusStrip
{

    private const float EPSILON = 0.01f;

    public override Vector3 GetPosition(float u, float v)
    {
        v = Mathf.Clamp(v, -1 + EPSILON, 1 - EPSILON);
        float x = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Cos(u);
        float y = (radius + v * width * Mathf.Cos(u / 2f)) * Mathf.Sin(u);
        float z = v * Mathf.Sin(u / 2f);

        return axis.MultiplyPoint(new Vector3(x, y, z));
    }
}
