using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{ 
    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player") 
        {
            // kill the player 
        }
    }

    private void Update()
    {
        
    }
}
