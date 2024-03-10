using UnityEngine;

[System.Serializable]
public class Player
{
    [Header("Player Settings")]
    [Range(0, 10)] public float fspeed = 1f;
    [Range(0, 10)] public float hspeed = 1f;
    [Range(0, 15)] public float hacceleration = 10f;
    [Range(0, 0.3f)] public float hfriction = 0.02f;

    private readonly GameObject gameObject;

    private readonly float upos;
    private readonly float vpos;
    private readonly float hpos;
    private readonly bool inverted;

    private float vspeed;

    public float UPos { get; set; }
    public float VSpeed
    {
        get => vspeed;
        set
        {
            vspeed = Mathf.Clamp(value, -hspeed * 0.2f * fspeed, hspeed * 0.2f * fspeed);
        }
    }
    public float HPos { get; set; }
    public bool Inverted { get; set; }
    public GameObject GameObject { get; set; }

}