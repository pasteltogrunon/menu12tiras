using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobiusStripTwisted : MobiusStrip
{

    private const float EPSILON = 0.01f;

    [Header("Mobius Strip Structure")]
    [Range(0, 20)] public float separation;

    public override Vector3 GetPosition(float u, float v)
    {
        v = Mathf.Clamp(v, -1f + EPSILON, 1f - EPSILON);
        Vector2 FValue = Lemniscata(u);
        Vector2 GValue = LemniscataNormal(u);
        float x = FValue.x + v * width * Mathf.Cos(u / 2 - Mathf.PI / 4) * GValue.x;
        float y = FValue.y + v * width * Mathf.Cos(u / 2 - Mathf.PI / 4) * GValue.y;
        float z = v * width * Mathf.Sin(u / 2 - Mathf.PI / 4) - separation * Mathf.Sin(u) / 2;

        return axis.MultiplyPoint(new Vector3(x, y, z));
    }

    private Vector2 Lemniscata(float t)
    {
        float x = radius * Mathf.Cos(t) / (1 + Mathf.Sin(t) * Mathf.Sin(t));
        float y = radius * Mathf.Cos(t) * Mathf.Sin(t) / (1 + Mathf.Sin(t) * Mathf.Sin(t));
        return new Vector2(x, y);
    }

    private Vector2 LemniscataNormal(float t)
    {
        float DF_x = -radius * Mathf.Sin(t) * (Mathf.Sin(t) * Mathf.Sin(t) + 2 * Mathf.Cos(t) * Mathf.Cos(t) + 1) / Mathf.Pow(1 + Mathf.Sin(t) * Mathf.Sin(t), 2);
        float DF_y = -radius * (Mathf.Pow(Mathf.Sin(t), 4) + (1 + Mathf.Cos(t) * Mathf.Cos(t)) * Mathf.Sin(t) * Mathf.Sin(t) - Mathf.Cos(t) * Mathf.Cos(t)) / Mathf.Pow(1 + Mathf.Sin(t) * Mathf.Sin(t), 2);

        float norma = Mathf.Sqrt(DF_x * DF_x + DF_y * DF_y);

        float x = DF_x / norma;
        float y = DF_y / norma;

        return new Vector2(y, -x);
    }
}
