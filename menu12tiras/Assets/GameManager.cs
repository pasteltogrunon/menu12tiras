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
        player.VPos += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * player.hspeed;
        player.VPos = Mathf.Clamp(player.VPos, -1 + EPSILON, 1 - EPSILON);
        player.UPos += Time.fixedDeltaTime * player.fspeed;
        player.UPos += Time.fixedDeltaTime * player.fspeed;
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
