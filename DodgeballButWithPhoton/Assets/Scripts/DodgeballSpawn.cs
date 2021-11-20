using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballSpawn : MonoBehaviour
{
    public GameObject ballChild;
    public int timer = 0;

    private void Update()
    {
        if (ballChild.activeInHierarchy) return;
        else
        {
            if (timer > 999) Respawn();
            else timer++;
        }
    }

    void Respawn()
    {
        ballChild.SetActive(true);
        timer = 0;
    }
}
