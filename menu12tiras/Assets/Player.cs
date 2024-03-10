using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(0, 10)] public float fspeed = 1f;
    [Range(0, 10)] public float hspeed = 1f;
    [Range(0, 15)] public float hacceleration = 10f;
    [Range(0, 0.3f)] public float hfriction = 0.02f;
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

    private float vspeed;

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
    public GameObject GameObject { get; set; }

    public float VSpeed
    {
        get => vspeed;
        set
        {
            vspeed = Mathf.Clamp(value, -hspeed * 0.2f * fspeed, hspeed * 0.2f * fspeed);
        }
    }

    private void Start()
    {
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
        upos += Time.fixedDeltaTime * fspeed / GameManager.instance.GetMobiusStripRadius();
    }

    private void UpdateVPos()
    {
        float hAxis = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        vspeed += hAxis * Time.fixedDeltaTime * hacceleration * fspeed;
        if (hAxis == 0)
        {
            vspeed *= 1 - hfriction;
        }

        vpos = Mathf.Clamp(vpos + vspeed * Time.fixedDeltaTime, -1 + EPSILON, 1 - EPSILON);

        if (vpos == -1 + EPSILON)
        {
            vspeed = Mathf.Clamp(vspeed, 0, hspeed);
        }
        else if (vpos == 1 - EPSILON)
        {
            vspeed = Mathf.Clamp(vspeed, -hspeed, 0);
        }
    }

}