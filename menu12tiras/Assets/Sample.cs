using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (float t = 0; t < 8; t += 0.1f)
        {
            Vector3 start = new Vector3(0, 0, 0);
            Vector3 end = new Vector3(1, 1, 1);
            Vector3 point = Vector3.Lerp(start, end, t);
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
