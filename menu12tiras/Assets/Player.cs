using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[System.Serializable]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(0, 20)] public float maxFSpeed = 16f;
    [Range(0, 10)] public float maxHSpeed = 1f;
    [Range(0, 5)] public float hacceleration = 10f;
    [Range(0, 1)] public float speedPow = 0.5f;
    [Range(0, 5)] public float negativeMultiplier = 3;
    [Range(0, 0.3f)] public float hfriction = 0.02f;
    [Range(0, 3f)] public float fhighAcceleration = 0.4f;
    [Range(0, 3f)] public float flowAcceleration = 0.05f;
    [Range(0, 20)] public float speedOfChange = 12f;


    [Header("Speed FX")]
    public AudioMixer mixer;
    public VisualEffect speedLines;
    public Camera camera;
    [Range(0, 10)] public float inversionCooldown = 4f;

    [Header("Rayo Settings")]
    public GameObject lookAtFollow;

    private const float EPSILON = GameManager.EPSILON;

    private float upos;
    private float vpos;
    private float hpos;
    private bool inverted;
    private bool inversionOnCooldown;
    private float nextInversion;

    private float uspeed;
    private float vspeed;
    private int coins;

    [Header("Inversion")]
    [SerializeField] private AudioSource inversionSource;
    [SerializeField] private ParticleSystem inversionVFX;

    [Header("Die")]
    [SerializeField] private AudioSource dieSource;
    [SerializeField] private AudioClip dieThrow;
    [SerializeField] private AudioClip[] dieClips = new AudioClip[6];

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite[] dieSprites = new Sprite[6];
    [SerializeField] private Sprite[] dieFaces = new Sprite[6];
    [SerializeField] private ParticleSystem throwDieParticles;

    public bool Invincible;
    private float invincibleTime;
    [SerializeField] private AudioSource invincibleSource;
    [SerializeField] private GameObject invinciSphere;

    private float cervezedTime;
    private bool cervecezed;
    public Volume cervezedPostProcess;


    public float UPos
    {
        get => upos;
        set => upos = value;
    }
    public float VPos
    {
        get => vpos;
        set => vpos = value;
    }
    public float HPos
    {
        get => hpos;
        set => hpos = value;
    }

    public bool Inverted
    {
        get => inverted;
        set => inverted = value;
    }
    public bool InversionOnCooldown
    {
        get => inversionOnCooldown;
        set => inversionOnCooldown = value;
    }
    public GameObject GameObject
    {
        get => gameObject;
    }

    public float USpeed
    {
        get => uspeed;
        set
        {
            if(!(Invincible && value < uspeed))
            {

                mixer.SetFloat("Volume1", Mathf.Clamp(34 * uspeed - 80, -100, 0));
                mixer.SetFloat("Volume2", Mathf.Clamp(17 * uspeed - 80, -100, 0));
                mixer.SetFloat("Volume3", Mathf.Clamp(8 * uspeed - 80, -100, 0));

                speedLines.SetFloat("SpawnRate", Mathf.Clamp(8 * uspeed, 0, 100));
                transform.GetChild(0).GetComponent<Animator>().speed = 1 + 0.1f * uspeed;

                uspeed = Mathf.Clamp(value, 0, maxFSpeed);

                onSpeedChanged?.Invoke(uspeed);
            }
        }
    }


    public float VSpeed
    {
        get => vspeed;
        set
        {
            vspeed = Mathf.Clamp(value, -maxHSpeed * 0.2f * maxFSpeed, maxHSpeed * 0.2f * maxFSpeed);
        }
    }

    public int Coins
    {
        get => coins;
        set
        {
            coins = value;

            onCoinsChanged?.Invoke(value);

            if (value >= 10)
            {
                coins = value - 10;

                StartCoroutine(throwDies());
            }
        }
    }

    public static event Action<float> onSpeedChanged;
    public static event Action<int> onCoinsChanged;

    public delegate void OnPlayerInverted();
    public static OnPlayerInverted onPLayerInverted;


    public void Start()
    {
        USpeed = 2;
        cervecezed = false;
        hpos = 0.1f;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        coins = 0;
    }

    private void Update()
    {
        CheckInversionCooldown();
        CheckInversionState();
        CheckCrouchState();
    }

    private void FixedUpdate()
    {
        UpdatePlayerPosition();
    }

    private void CheckInversionCooldown()
    {
        if (inversionOnCooldown)
        {
            nextInversion -= Time.deltaTime;
            inversionOnCooldown = nextInversion > 0;
        }
    }

    private void CheckInversionState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!inversionOnCooldown)
            {
                inverted = !inverted;
                inversionSource.Play();
                inversionVFX.Play();
                inversionOnCooldown = true;
                nextInversion = inversionCooldown;
                onPLayerInverted?.Invoke();
            }
        }
    }

    private void CheckCrouchState()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            hpos = 0.05f;
            transform.localScale -= new Vector3(0, 0.05f, 0);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            hpos = 0.1f;
            transform.localScale += new Vector3(0, 0.05f, 0);
        }
    }

    private void UpdatePlayerPosition()
    {
        UpdateUPos();
        UpdateVPos();
        Vector3 currentPoint = GameManager.instance.GetMobiusStripPosition(upos, vpos);
        Vector3 normal = GameManager.instance.GetMobiusStripNormal(upos, vpos);
        Vector3 lookAt = GameManager.instance.GetMobiusStripLookAt(upos, vpos);
        Vector3 offset = hpos * (inverted ? -1 : 1) * normal;
        transform.SetPositionAndRotation(currentPoint + offset, Quaternion.LookRotation(lookAt, (inverted ? -1 : 1) * normal));

        lookAtFollow.transform.SetPositionAndRotation(currentPoint + offset, Quaternion.LookRotation(lookAt, normal));
    }

    private void UpdateUPos()
    {
        USpeed += (USpeed < speedOfChange ? fhighAcceleration : flowAcceleration) * Time.fixedDeltaTime;
        upos += Time.fixedDeltaTime * uspeed / GameManager.instance.GetMobiusStripRadius();
    }

    private void UpdateVPos()
    {
        float hAxis = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        hAxis *= (cervecezed ? -1 : 1);
        if (hAxis * vspeed < 0)
            hAxis *= negativeMultiplier;

        vspeed += hAxis * Time.fixedDeltaTime * hacceleration * Mathf.Pow(uspeed, speedPow);
        if (hAxis == 0)
        {
            vspeed *= 1 - hfriction;
        }

        vpos = Mathf.Clamp(vpos + vspeed * Time.fixedDeltaTime, -1 + EPSILON, 1 - EPSILON);

        if (vpos == -1 + EPSILON)
        {
            vspeed = Mathf.Clamp(vspeed, 0, maxHSpeed);
        }
        else if (vpos == 1 - EPSILON)
        {
            vspeed = Mathf.Clamp(vspeed, -maxHSpeed, 0);
        }
        else
        {
            vspeed = Mathf.Clamp(vspeed, -maxHSpeed, maxHSpeed);
        }
    }


    IEnumerator throwDies()
    {
        dieSource.PlayOneShot(dieThrow);
        yield return new WaitForSeconds(0.5f);
        int number = Die.throwDie(this) - 1;
        renderer.gameObject.SetActive(true);
        renderer.sprite = dieFaces[number];
        throwDieParticles.Play();
        yield return new WaitForSeconds(0.5f);
        dieSource.PlayOneShot(dieClips[number]);
        renderer.sprite = dieSprites[number];
        yield return new WaitForSeconds(1f);
        renderer.gameObject.SetActive(false);

    }

    public void Cervez(float time)
    {
        if (cervezedTime > 0)
        {
            cervezedTime = Mathf.Max(time, invincibleTime);
        }
        else
        {
            cervezedTime = Mathf.Max(time, invincibleTime);
            StartCoroutine(startCervez());
        }
    }


    IEnumerator startCervez()
    {
        cervecezed = true;
        mixer.SetFloat("NormalVolume", -80);
        mixer.SetFloat("CervezedVolume", 0);
        cervezedPostProcess.weight = 1;
        while (cervezedTime > 0)
        {
            cervezedTime -= Time.deltaTime;
            yield return null;
        }
        if(!Invincible)
            mixer.SetFloat("NormalVolume", 0);
        mixer.SetFloat("CervezedVolume", -80);
        cervezedPostProcess.weight = 0;
        cervecezed = false;
    }


    public void Invinivilize(float time)
    {
        if(invincibleTime > 0)
        {
            invincibleSource.Play();
            invincibleTime = Mathf.Max(time, invincibleTime);
        }
        else
        {
            invincibleSource.Play();
            invincibleTime = Mathf.Max(time, invincibleTime);
            StartCoroutine(startInvincibility());
        }
    }

    IEnumerator startInvincibility()
    {
        Invincible = true;
        mixer.SetFloat("NormalVolume", -80);
        mixer.SetFloat("CervezedVolume", -80);
        invinciSphere.SetActive(true);
        while(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
            yield return null;
        }
        invinciSphere.SetActive(false);
        if(cervecezed)
        {
            mixer.SetFloat("CervezedVolume", 0);
        }
        else
        {
            mixer.SetFloat("NormalVolume", 0);
        }
        Invincible = false;
    }
}