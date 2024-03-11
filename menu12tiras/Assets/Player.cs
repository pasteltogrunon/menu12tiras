using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

[System.Serializable]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(0, 20)] public float maxFSpeed = 15f;
    [Range(0, 10)] public float maxHSpeed = 1f;
    [Range(0, 5)] public float hacceleration = 10f;
    [Range(0, 0.3f)] public float hfriction = 0.02f;
    [Range(0, 3f)] public float facceleration = 0.2f;

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
            mixer.SetFloat("Volume1", Mathf.Clamp(34 * uspeed - 80, -100, 0));
            mixer.SetFloat("Volume2", Mathf.Clamp(17 * uspeed - 80, -100, 0));
            mixer.SetFloat("Volume3", Mathf.Clamp(8 * uspeed - 80, -100, 0));

            speedLines.SetFloat("SpawnRate", Mathf.Clamp(8 * uspeed, 0, 100));

            uspeed = Mathf.Clamp(value, 0, maxFSpeed);
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
            Debug.Log("Coins: " + coins);
        }
    }

    private void Start()
    {
        USpeed = 1;
        hpos = 0.1f;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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

                inversionOnCooldown = true;
                nextInversion = inversionCooldown;
            }
        }
    }

    private void CheckCrouchState()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            hpos = 0.05f;
            transform.localScale -= new Vector3(0, 0.05f, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
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
        USpeed += facceleration * Time.fixedDeltaTime;
        upos += Time.fixedDeltaTime * uspeed / GameManager.instance.GetMobiusStripRadius();
    }

    private void UpdateVPos()
    {
        float hAxis = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        vspeed += hAxis * Time.fixedDeltaTime * hacceleration * uspeed;
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
    }

}