using UnityEngine;

[System.Serializable]
public class Player
{
    [Header("Player Settings")]
    [Range(0, 10)] public float fspeed = 1f;
    [Range(0, 10)] public float hspeed = 1f;

    [Header("Player Object")]
    private readonly GameObject gameObject;

    private readonly float upos;
    private readonly float vpos;
    private readonly float hpos;
    private readonly bool inverted;

    public float UPos { get; set; }
    public float VPos { get; set; }
    public float HPos { get; set; }
    public bool Inverted { get; set; }
    public GameObject GameObject { get; set; }
    
}