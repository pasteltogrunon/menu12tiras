using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(0, 10)] public float fspeed = 1f;
    [Range(0, 10)] public float hspeed = 1f;
    [Range(0, 15)] public float hacceleration = 10f;
    [Range(0, 0.3f)] public float hfriction = 0.02f;

    private const float EPSILON = GameManager.EPSILON;

    private float upos;
    private float vpos;
    private float hpos;
    private bool inverted;

    private float vspeed;
    private float uspeed;

    public float UPos { get; set; }
    public float VPos { get; set; }
    public float HPos { get; set; }
    public bool Inverted { get; set; }
    public GameObject GameObject { get; set; }

    public AudioMixer mixer;

    public float VSpeed
    {
        get => vspeed;
        set
        {
            vspeed = Mathf.Clamp(value, -hspeed * 0.2f * fspeed, hspeed * 0.2f * fspeed);
        }
    }

    public float USpeed
    {
        get => uspeed;
        set
        {
            mixer.SetFloat("Volume1", Mathf.Clamp(34 * uspeed - 80, -100, 0));
            mixer.SetFloat("Volume2", Mathf.Clamp(17 * uspeed -80, -100, 0));
            mixer.SetFloat("Volume3", Mathf.Clamp(8 * uspeed - 80, -100, 0));

            uspeed = value;
        }
    }


    private void Start()
    {
        hpos = 0.1f;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void Update()
    {
        CheckInversionState();
        CheckCrouchState();
    }

    private void FixedUpdate()
    {
        UpdatePlayerPosition();
    }

    private void CheckInversionState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inverted = !inverted;
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
    }

    private void UpdateUPos()
    {
        if (Input.GetKey(KeyCode.W)) USpeed += 3 * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.S)) USpeed -= 3 * Time.fixedDeltaTime;
        upos += Time.fixedDeltaTime * uspeed / GameManager.instance.GetMobiusStripRadius();
    }

    private void UpdateVPos()
    {
        float hAxis = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        vspeed += hAxis * Time.fixedDeltaTime * hacceleration * fspeed;
        if (hAxis == 0)
        {
            vspeed *= 1 - hfriction;
        }

        vpos = Mathf.Clamp(vpos + (inverted ? -1 : 1) * vspeed * Time.fixedDeltaTime, -1 + EPSILON, 1 - EPSILON);

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