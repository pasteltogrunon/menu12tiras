using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private const float EPSILON = 0.05f;

    public MobiusStripTwisted mobiusStrip = new();
    public Player player = new();
    public bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        mobiusStrip.GenerateMobiusStrip(mobiusStrip.material, mobiusStrip.uResolution, mobiusStrip.vResolution);
        StartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Inverted = !player.Inverted;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            player.HPos = 0.05f;
            player.GameObject.transform.localScale -= new Vector3(0, 0.05f, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            player.HPos = 0.1f;
            player.GameObject.transform.localScale += new Vector3(0, 0.05f, 0);
        }

        player.VPos += Input.GetAxis("Horizontal") * Time.deltaTime * player.hspeed;
        player.VPos = Mathf.Clamp(player.VPos, -1 + EPSILON, 1 - EPSILON);
        player.UPos += Time.deltaTime * player.fspeed;
        UpdatePlayerPosition();
    }

    private void StartPlayer()
    {
        player.GameObject = GameObject.Find("Player");
        player.HPos = 0.1f;
        player.GameObject.transform.position = mobiusStrip.GetPosition(player.UPos, player.VPos);
        player.GameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        player.GameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private void UpdatePlayerPosition()
    {
        Vector3 currentPoint = mobiusStrip.GetPosition(player.UPos, player.VPos);
        Vector3 normal = mobiusStrip.Normal(player.UPos, player.VPos);
        Vector3 lookAt = mobiusStrip.LookAt(player.UPos, player.VPos);
        Vector3 offset = player.HPos * (player.Inverted ? -1 : 1) * normal;
        player.GameObject.transform.SetPositionAndRotation(currentPoint + offset, Quaternion.LookRotation(lookAt, normal));
    }
}
