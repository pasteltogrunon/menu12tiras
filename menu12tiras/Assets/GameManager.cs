using System.Collections;
using UnityEngine;

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
    public bool debug = false;

    private void Awake()
    {
        instance = this;
    }

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
        //StartCoroutine(SpawnCoins());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* IEnumerator SpawnCoins()
    {
        while (true)
        {
            int CoinIndex = 0;
            //int CoinIndex = Random.Range(-1, 2);
            for (int i = 0; i < 3; i++)
            {
                GameObject temp = Instantiate(coin);
                float u = GameObject.Find("Player").GetComponent<Player>().UPos + 0.3f + i * EPSILON;
                float v = CoinIndex + (CoinIndex < 0 ? 1 : (CoinIndex > 0 ? -1 : 0)) * EPSILON * 10;
                temp.GetComponent<Coin>().Init(u, v);
            }
            yield return new WaitForSeconds(4f);
        }
    } */

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
}
