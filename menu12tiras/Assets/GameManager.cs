using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

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

    public Player player;
    public PauseMenu pauseMenu;
    public AudioMixer mixer;
    public AudioSource[] musicSources = new AudioSource[5];

    [Header("Energy")]
    [SerializeField] private float maxEnergyDecrease = 41f;
    [SerializeField] private float maxEnergyIncrease = 12f;
    private float deltaEnergy;

    [SerializeField] private float energy;
    [SerializeField] private AudioSource fullEnergy;
    private bool alreadyShouted;

    public float Energy
    {
        get => energy;
        set
        {
            if (value < 75) alreadyShouted = false;
            if (value >= 100 && !alreadyShouted)
            {
                alreadyShouted = true;
                fullEnergy.Play();
            }
            energy = Mathf.Clamp(value, 0, 100);
        }
    }

    private float v0min;
    private float v0max;
    private float cDec;
    private float cInc;

    public float TotalTime;
    public float TotalDistance;

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

        TotalTime = 0f;
        
        energy = 80.5f;
        deltaEnergy = 0f;
        v0min = 6f;
        v0max = 6.6f;
        cDec = Mathf.PI / (2 * (v0min - 4f));
        cInc = (-1) * Mathf.PI / (2 * (v0max - player.speedOfChange));

        mixer.SetFloat("GameMusicVolume", 0);
        mixer.SetFloat("NormalVolume", 0);
        mixer.SetFloat("CervezedVolume", -80);
        mixer.SetFloat("InvincibleVolume", 0);
        mixer.SetFloat("PauseMusicVolume", -80);

        foreach(AudioSource s in musicSources)
        {
            s.Play();
        }

        foreach (AudioSource s in musicSources)
        {
            s.time = 0;
        }

        StartCoroutine(syncMusic());

    }

    public void Restart()
    {
        TotalTime = 0f;
        
        energy = 80.5f;
        deltaEnergy = 0f;
        v0min = 6f;
        v0max = 6.6f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TotalTime += Time.fixedDeltaTime;

        if (TotalTime >= 8)
        {
            UpdateDeltaEnergy();
            Energy += deltaEnergy * Time.fixedDeltaTime;
            if (Energy <= 0 && !pauseMenu.GameFinished) pauseMenu.GameOver();
        }

        TotalDistance += 4 * player.USpeed * Time.fixedDeltaTime;
    }

    private void UpdateDeltaEnergy()
    {
        float playerVelocity = player.USpeed;

        if (playerVelocity < 4f) deltaEnergy = -maxEnergyDecrease;
        else if (player.speedOfChange < playerVelocity) deltaEnergy = maxEnergyIncrease;

        else if (playerVelocity < v0min)
        {
            deltaEnergy = Mathf.Clamp( ( maxEnergyDecrease * Mathf.Sin( cDec * (playerVelocity - 4f) ) ) - maxEnergyDecrease, -maxEnergyDecrease, maxEnergyIncrease);
        }
        else if (v0max < playerVelocity )
        {
            deltaEnergy = Mathf.Clamp( ( maxEnergyIncrease * Mathf.Sin( cInc * (playerVelocity - player.speedOfChange) ) ) + maxEnergyIncrease, -maxEnergyDecrease, maxEnergyIncrease);
        }

        else deltaEnergy = 0;
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

    IEnumerator syncMusic()
    {
        while(true)
        {
            foreach (AudioSource s in musicSources)
            {
                s.time = musicSources[0].time;
            }
            yield return new WaitForSeconds(2);
        }
    }
}
