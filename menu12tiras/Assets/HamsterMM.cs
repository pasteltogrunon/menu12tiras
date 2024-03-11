using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterMM : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 10*Time.time, 0);
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(Time.time), transform.localPosition.z);
    }
}
