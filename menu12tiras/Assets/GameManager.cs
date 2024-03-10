using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private const float EPSILON = 0.05f;

    public MobiusStrip mobiusStrip = new();
    public Player player = new();
    public bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        mobiusStrip.GenerateMobiusStrip();
        StartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Inverted = !player.Inverted;
        }
    }

    void FixedUpdate()
    {
        player.VPos += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * player.hspeed;
        player.VPos = Mathf.Clamp(player.VPos, -1 + EPSILON, 1 - EPSILON);
        player.UPos += Time.fixedDeltaTime * player.fspeed;
        UpdatePlayerPosition();
    }

    private void StartPlayer()
    {
        player.GameObject = GameObject.Find("Player");
        player.GameObject.transform.position = mobiusStrip.GetPosition(player.UPos, player.VPos);
        player.GameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        player.GameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private void UpdatePlayerPosition()
    {
        Vector3 currentPoint = mobiusStrip.GetPosition(player.UPos, player.VPos);
        Vector3 normal = mobiusStrip.Normal(player.UPos, player.VPos);
        Vector3 lookAt = mobiusStrip.LookAt(player.UPos, player.VPos);
        Vector3 offset = 0.1f * (player.Inverted ? -1 : 1) * normal;
        player.GameObject.transform.SetPositionAndRotation(currentPoint + offset, Quaternion.LookRotation(lookAt, normal));
    }

    //Debugging
    private void OnDrawGizmos()
    {
        if (!debug) return;

        for (float u = 0; u < 2 * Mathf.PI; u += 0.01f)
        {
            for (float v = -1; v < 1; v += 0.05f)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(mobiusStrip.GetPosition(u, v), mobiusStrip.GetPosition(u + 0.1f, v));
            }
        }

        float ustep = 2 * Mathf.PI / mobiusStrip.uResolution;
        float hstep = 2f * mobiusStrip.boundHeight / mobiusStrip.hResolution;

        float u_ = 0;
        float h = -mobiusStrip.boundHeight;

        while (u_ < 2 * Mathf.PI)
        {
            while (h < 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(mobiusStrip.GetPosition(u_, -1) + h * mobiusStrip.Normal(u_, -1 + EPSILON), 0.01f);
                Gizmos.DrawSphere(mobiusStrip.GetPosition(u_, 1) + h * mobiusStrip.Normal(u_, 1 - EPSILON), 0.01f);
                h += hstep;
            }
            h = -mobiusStrip.boundHeight;
            u_ += ustep;
        }
    }
}
