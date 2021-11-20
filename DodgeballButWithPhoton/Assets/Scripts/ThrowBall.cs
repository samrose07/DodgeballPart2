using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ThrowBall : MonoBehaviourPun
{
    public int Timer = 0;
    bool timeGo = false;
    public Camera cam;
    public float upwardForce, throwForce;
    public Transform ballSpawnPoint;
    GameObject ballThatWasDisabled;
    public PickupObject pickupBall, pickupWrench;
    public GameObject player;
    public GameObject dodgeball;
    public GameObject wrench;

    public GameObject heldBall;
    public GameObject heldWrench;
    public Transform holdingPosition;

    public GameObject ballHolding;
    public GameObject wrenchHolding;
    
    public bool pickedUpBall = false;
    public bool pickedUpWrench = false;
    public ThrowBall throwBall;
    public bool isViewMine = false;
    public PlayerManager pm;
    public void Start()
    {
        /*ballHolding = PhotonNetwork.Instantiate("DodgeballHeld", holdingPosition.position, holdingPosition.rotation);
        ballHolding.transform.parent = player.transform;
        ballHolding.SetActive(false);
        
        wrenchHolding = PhotonNetwork.Instantiate("WrenchHeld", holdingPosition.position, holdingPosition.rotation);
        wrenchHolding.transform.parent = player.transform;
        wrenchHolding.SetActive(false);*/
        pm = gameObject.GetComponent<PlayerManager>();
    }
    public void Update()
    {
        if (pm != null)
        {
            isViewMine = pm.photonView.IsMine;
            if (isViewMine)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    ThrowTheBall();
                    
                }
            }
        }
        else
        {
            pm = gameObject.GetComponent<PlayerManager>();
        }
    }

    public void ThrowTheBall()
    {
        Vector3 targetPoint;
        if(pickedUpBall == true || pickedUpWrench == true)
        {
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                targetPoint = hit.point;

                
                if (pickedUpBall == true)
                {
                    /*GameObject dodgeballSpawn = PhotonNetwork.Instantiate("Dodgeball", player.transform.position, player.transform.rotation);
                    Debug.Log("dodgeball spawned");
                    dodgeballSpawn.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
                    // pickupBall.pickedUpBall = false;*/
                    Vector3 direction = targetPoint - ballSpawnPoint.position;
                    GameObject ballSpawn = PhotonNetwork.Instantiate("Dodgeball", ballSpawnPoint.position, Quaternion.identity);
                    ballSpawn.GetComponent<Rigidbody>().AddForce(direction.normalized * throwForce, ForceMode.Impulse);
                    ballSpawn.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
                    pickedUpBall = false;
                    //ballHolding.SetActive(false);
                }
                if (pickedUpWrench == true)
                {
                    /*GameObject wrenchSpawn = PhotonNetwork.Instantiate("Wrench", player.transform.position, player.transform.rotation);
                    wrenchSpawn.GetComponent<Rigidbody>().AddForce(transform.right * 1000);*/
                    // pickupWrench.pickedUpWrench = false;
                    Vector3 direction = targetPoint - ballSpawnPoint.position;
                    GameObject ballSpawn = PhotonNetwork.Instantiate("Wrench", ballSpawnPoint.position, Quaternion.identity);
                    ballSpawn.GetComponent<Rigidbody>().AddForce(direction.normalized * throwForce, ForceMode.Impulse);
                    pickedUpWrench = false;
                    //wrenchHolding.SetActive(false);
                }
            }
        }
        
    }
    public void OnTriggerEnter(Collider other)
    {
        // Destroy(gameObject);
        //Debug.Log("dead_dodgeball");
        
        if (isViewMine)
        {
           // Debug.Log(other.tag);
            if(other.tag == "dodgeball")
            {
                //PhotonNetwork.Destroy(other.gameObject);
                pickedUpBall = true;
                photonView.RPC("DestroyObject", RpcTarget.AllBuffered, other.gameObject.name);
               // Debug.Log("ball" + pickedUpBall);
                //ballHolding.SetActive(true);
            }
            if(other.tag == "wrench")
            {
                pickedUpWrench = true;
                photonView.RPC("DestroyObject", RpcTarget.AllBuffered, other.gameObject.name);
                //Debug.Log("wrench" + pickedUpWrench);
                //wrenchHolding.SetActive(true);
            } 
        }
    }


    [PunRPC]
    void DestroyObject(string name)
    {
        //GameObject.Destroy(GameObject.Find(name));
        GameObject gm = GameObject.Find(name);
        ballThatWasDisabled = gm;
        gm.SetActive(false);
    }
    /*    public void HoldingBall()
    {
        if (pickupBall.pickedUpBall)
        {
            ballHolding = Instantiate(heldBall, holdingPosition.position, holdingPosition.rotation);
            ballHolding.transform.parent = player.transform;
        }
    }
    public void HoldingWrench()
    {
        if (pickupWrench.pickedUpWrench)
        {
            wrenchHolding = Instantiate(heldWrench, holdingPosition.position, holdingPosition.rotation);
            wrenchHolding.transform.parent = player.transform;
        }
    }*/
}
