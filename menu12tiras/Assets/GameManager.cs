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
    public bool debug = false;

    [SerializeField] private Player player;

    [Header("Energy")]
    [SerializeField] private float initialEnergyDecrease = 0.3f;
    [SerializeField] private float energyDecreasePerTime = 0.05f;
    [SerializeField] private float energyGrowthPerSpeed = 0.3f;

    private float totalTime;
    private float energylevel;

    public float EnergyLevel
    {
        get => energylevel;
        set
        {
            energylevel = Mathf.Clamp(value, 0, 100);
        }
    }

    public float EnergyDecrease
    {
        get => initialEnergyDecrease + energyDecreasePerTime * totalTime;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (mobiusStripType)
        {
            case MobiusStripType.Default:
                mobiusStrip = mobiusStripDefault;
                break;
            case MobiusStripType.Twisted:
                mobiusStrip = mobiusStripTwisted;
                break;
        }
        mobiusStrip.GenerateMobiusStrip();

        EnergyLevel = 75;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        totalTime += Time.fixedDeltaTime;
        EnergyLevel -= EnergyDecrease;
        EnergyLevel += energyGrowthPerSpeed * player.USpeed;
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
