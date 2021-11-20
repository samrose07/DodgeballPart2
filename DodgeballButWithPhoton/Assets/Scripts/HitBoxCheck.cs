using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCheck : MonoBehaviour
{
    //public GameObject thirdPersonControllerObject;
    public PlayerController playerControllerScript;
    bool viewMine = false;
    private void Update()
    {
        viewMine = playerControllerScript.viewIsMine;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (viewMine)
        {
            //print("I GOT HERE HELLO AND THE COLLISION WAS WITH TAG :" + collision.gameObject.name);
            if (other.gameObject.tag == "ThrownBall" || other.gameObject.tag == "ThrownWrench")
            {
                
                PlayerManager pm = gameObject.GetComponentInParent<PlayerManager>();
                pm.ImOut();
                
            }
        }
    }
    
}
