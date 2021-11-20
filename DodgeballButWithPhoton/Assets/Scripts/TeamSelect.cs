using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelect : MonoBehaviour
{
    public GameManager gm;
    public bool inTrigger = false;
    public float timer;
    public List<GameObject> listOfPlayers;
    public byte setTeamNumber;

    private void Start()
    {
        listOfPlayers = new List<GameObject>();
        listOfPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerManager>().team = setTeamNumber;
        }
    }

    private void Update()
    {

        if (timer >= 5.0f)
        {
            //gm.StartFromTeamSelect();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            timer = 0;
            inTrigger = false;
        }
    }
}
