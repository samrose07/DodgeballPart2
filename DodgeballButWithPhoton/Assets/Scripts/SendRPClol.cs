using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SendRPClol : MonoBehaviourPunCallbacks
{

    public void SendTheRPC(string rpc, int viewID, byte team, Player player)
    {
        print("here, also " +player.NickName + " is the player im gonna send this to");
        photonView.RPC(rpc, player, viewID, team);
    }

    public void SendDestroy(string rpc, string name)
    {
        photonView.RPC(rpc, RpcTarget.AllBuffered, name);
    }

    public void SendTeamOutUpdate(string rpc, int team, string name, bool add)
    {
        photonView.RPC(rpc, RpcTarget.All, team, name, add);
    }
    [PunRPC]
    void SendTeamOut(int team, string name, bool add)
    {
        GameObject obj = GameObject.Find(name);
        //print(name + " is the gameObject we found");
        if (obj == null) return;
        //print("hey what is really good");
        PlayerManager pm;
        pm = obj.GetComponent<PlayerManager>();
        if (pm == null) return;
        //print("i got hither");
        if(pm != null)
        {
            if (add)
            {
                pm.UpdatePlayersOut(true, team);
            }
            else
            {
                pm.UpdatePlayersOut(false, team);
            }
        }
        
        
    }

    [PunRPC]
    void NotOutNoMo(int viewID, byte checkedTeam)
    {
        Debug.Log("I just ran i think");
        GameObject gameObj = PhotonView.Find(viewID).gameObject;
        Debug.Log(gameObj.name);
        PlayerManager localPM;
        localPM = gameObj.GetComponentInParent<PlayerManager>();
        if (localPM == null) return;    
        if(localPM != null)
        {
            if (localPM.team == checkedTeam)
            {
                localPM.ImBackIn();
            }
        }


    }

    [PunRPC]
    void DestroyTheObject(string name)
    {
        //GameObject.Destroy(GameObject.Find(name));
        GameObject gm = GameObject.Find(name);
        PhotonNetwork.Destroy(gm);

    }
}
