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

    private const float EPSILON = 0.05f;
    public MobiusStripType mobiusStripType;
    public MobiusStripDefault mobiusStripDefault = new();
    public MobiusStripTwisted mobiusStripTwisted = new();
    private MobiusStrip mobiusStrip;
    public Player player = new();
    public Coin coin;
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
        player.VPos += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * player.hspeed * 0.2f * player.fspeed;
        player.VPos = Mathf.Clamp(player.VPos, -1 + EPSILON, 1 - EPSILON);
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

    public GameObject coinPre;

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            int CoinIndex = Random.Range(-1, 2);
            for (int i = 0; i < 3; i++)
            {
                GameObject temp = Instantiate(coinPre);
                float u = player.UPos + 0.3f + i * 0.05f;
                Vector3 position = mobiusStrip.GetPosition(u, CoinIndex);
                Vector3 lookAt = mobiusStrip.GetPosition(u, CoinIndex);
                Vector3 normal = mobiusStrip.Normal(u, CoinIndex);
                temp.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt, normal));
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
