using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum MobiusStripType
    {
        Default,
        Twisted
    }

    public static GameManager instance;

    private const float EPSILON = 0.05f;
    public MobiusStripType mobiusStripType;
    public MobiusStripDefault mobiusStripDefault = new();
    public MobiusStripTwisted mobiusStripTwisted = new();
    private MobiusStrip mobiusStrip;
    public Player player = new();

    public GameObject coin;
    public bool debug = false;

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
        StartPlayer();
        StartCoroutine(SpawnCoins());
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

    }

    void FixedUpdate()
    {
        player.UPos += Time.fixedDeltaTime * player.fspeed / mobiusStrip.radius;

        //VPos calculation
        float hAxis = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        Debug.Log(hAxis);
        player.VSpeed +=  hAxis * Time.fixedDeltaTime * player.hacceleration * player.fspeed;
        if(hAxis == 0)
        {
            player.VSpeed *= (1 - player.hfriction);
        }

        player.VPos = Mathf.Clamp(player.VPos + player.VSpeed * Time.fixedDeltaTime, -1 + EPSILON, 1 - EPSILON);

        if(player.VPos == -1 + EPSILON)
        {
            player.VSpeed = Mathf.Clamp(player.VSpeed, 0, player.hspeed);
        }
        else if(player.VPos == 1 - EPSILON)
        {
            player.VSpeed = Mathf.Clamp(player.VSpeed, -player.hspeed, 0);
        }

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

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            int CoinIndex = 0;
            //int CoinIndex = Random.Range(-1, 2);
            for (int i = 0; i < 3; i++)
            {
                GameObject temp = Instantiate(coin);
                float u = player.UPos + 0.3f + i * EPSILON;
                Vector3 position = mobiusStrip.GetPosition(u, CoinIndex + (CoinIndex < 0 ? 1 : (CoinIndex > 0 ? -1 : 0)) * EPSILON * 10);
                Vector3 lookAt = mobiusStrip.GetPosition(u, CoinIndex);
                Vector3 normal = mobiusStrip.Normal(u, CoinIndex);
                temp.transform.SetPositionAndRotation(position + 0.2f * normal, Quaternion.LookRotation(lookAt, normal));
            }
            yield return new WaitForSeconds(4f);
        }

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
