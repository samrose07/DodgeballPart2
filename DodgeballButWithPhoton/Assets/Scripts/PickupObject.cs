using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public bool pickedUpBall = false;
    public bool pickedUpWrench = false;
    public ThrowBall throwBall;
    // public GameObject ballHolding;
    // public GameObject wrenchHolding;

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        //Debug.Log("dead_dodgeball");
        if(gameObject.CompareTag("dodgeball"))
        {
            pickedUpBall = true;
            Debug.Log("ball" + pickedUpBall);
            throwBall.ballHolding.SetActive(true);
        }
        if(gameObject.CompareTag("wrench"))
        {
            pickedUpWrench = true;
            Debug.Log("wrench" + pickedUpWrench);
            throwBall.wrenchHolding.SetActive(true);
        } 
    }
}
