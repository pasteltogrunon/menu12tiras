using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum MobiusStripType
    {
        Default,
        Twisted
    }

    public static GameManager instance;

    public const float EPSILON = 0.05f;
    public MobiusStripType mobiusStripType;
    public MobiusStripDefault mobiusStripDefault = new();
    public MobiusStripTwisted mobiusStripTwisted = new();
    private MobiusStrip mobiusStrip;

    public GameObject coin;
    public GameObject wall;
    public GameObject slidingBar;
    public GameObject electricField;
    public bool debug = false;

    public Player Player;

    [Header("Energy")]
    [SerializeField] private float initialEnergyDecrease = 0.3f;
    [SerializeField] private float energyDecreasePerTime = 0.05f;
    [SerializeField] private float energyGrowthPerSpeed = 0.3f;
    [SerializeField] private AudioSource fullEnergy;
    private bool alreadyShouted;

    public float TotalTime;
    private float energylevel;

    public float EnergyLevel
    {
        get => energylevel;
        set
        {
            if (value < 75) alreadyShouted = false;
            if (value >= 100 && !alreadyShouted)
            {
                alreadyShouted = true;
                fullEnergy.Play();
            }
            energylevel = Mathf.Clamp(value, 0, 100);
        }
    }

    public float EnergyDecrease
    {
        get => initialEnergyDecrease + energyDecreasePerTime * TotalTime;
    }

    private void Awake()
    {
        instance = this;
        switch (mobiusStripType)
        {
            case MobiusStripType.Default:
                mobiusStrip = mobiusStripDefault;
                break;
            case MobiusStripType.Twisted:
                mobiusStrip = mobiusStripTwisted;
                break;
        }
        electricField = new();
        electricField.AddComponent<ElectricFloor>();

    }

    // Start is called before the first frame update
    void Start()
    {
        mobiusStrip.GenerateMobiusStrip();

        EnergyLevel = 75;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        TotalTime += delta;
        EnergyLevel -= EnergyDecrease * delta;
        EnergyLevel += energyGrowthPerSpeed * Player.USpeed * delta;
    }


    public Vector3 GetMobiusStripPosition(float u, float v)
    {
        return mobiusStrip.GetPosition(u, v);
    }

    public Vector3 GetMobiusStripNormal(float u, float v)
    {
        return mobiusStrip.Normal(u, v);
    }

    public Vector3 GetMobiusStripLookAt(float u, float v)
    {
        return mobiusStrip.LookAt(u, v);
    }

    public float GetMobiusStripRadius()
    {
        return mobiusStrip.radius;
    }
}
