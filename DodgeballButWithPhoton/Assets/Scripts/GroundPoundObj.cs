using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundObj : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ground") Destroy(gameObject);
        //Debug.Log("Collider is " + collision.gameObject.tag);
    }
}
