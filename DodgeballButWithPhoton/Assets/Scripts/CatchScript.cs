using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class CatchScript : MonoBehaviourPunCallbacks
{
     public GameObject catchSprite;
    public PlayerController pc;
    bool isMine = false;
    public SendRPClol rpcScript;
    private void Update()
    {
        isMine = pc.viewIsMine;
    }
    /*
     * how this script should work:
     * 
     *          ball enters hitbox
     *          leftClick pressed while inside trigger
     *          ball caught
     *          adds ball to player inventory if not already there
     *          gets out player back in if on team, handle that in pm tho
     */
    private void OnTriggerStay(Collider other)
    {
        if (isMine)
        {
            if (other.gameObject.tag == "ThrownBall")
            {
                /*
                 * so this is a lot, here's what happens:
                 * script changes dodgeball's name so that the wrong ball isn't destroyed in the scene.
                 * scripts then finds the team number of the player with this script attached
                 * and then it gets the players team members. probably ends up getting itself too,
                 * so i run a foreach loop to check each player's out status.
                 * if they are out when a ball is caught,
                 * they be back in!
                 */
                catchSprite.SetActive(true);
                if (Input.GetButtonDown("Fire2"))
                {
                    other.gameObject.name = other.gameObject.name + "collided";
                    //photonView.RPC("DestroyObject", RpcTarget.AllBuffered, other.gameObject.name);
                    rpcScript.SendDestroy("DestroyTheObject", other.gameObject.name);
                    PlayerManager pm = gameObject.GetComponentInParent<PlayerManager>();
                    Player[] membersOnMyTeam;
                    byte team = pm.team;
                    byte myTeam = pm.team;
                    PhotonView pv = gameObject.GetComponentInParent<PhotonView>();
                    int myID = pv.ViewID;
                    var myOwner = pv.Owner;
                    PhotonTeamsManager.Instance.TryGetTeamMembers(team, out membersOnMyTeam);
                    GameObject viewGameObject;
                    var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
                    foreach (var pViews in photonViews)
                    {
                        var pla = pViews.Owner;

                        if (pla == myOwner) continue;
                        if (pla != null)
                        {
                            viewGameObject = pViews.gameObject;
                            print(viewGameObject.name);
                            PlayerManager localPM;
                            localPM = viewGameObject.GetComponent<PlayerManager>();
                            if (localPM != null) print(localPM.gameObject.name);
                            if (localPM == null) continue;
                            team = localPM.team;
                            if (team != myTeam) continue;
                            if (team == myTeam)
                            {
                                print("my team is my team");
                                int viewID = viewGameObject.GetPhotonView().ViewID;
                                if (viewID == myID) continue;
                                else
                                {

                                    print("not my view id");
                                    if (localPM.amIOut)
                                    {
                                        
                                        rpcScript.SendTheRPC("NotOutNoMo",viewID, team, pViews.Owner);
                                        //photonView.RPC("NotOutNoMo", pViews.Owner, viewID, myTeam, team);
                                        //localPM.ImBackIn();
                                    }
                                }
                            }
                        }
                    }
                    /*foreach (Player p in membersOnMyTeam)
                    {
                        string playernickname = p.NickName;
                        int playerNumber = p.GetPlayerNumber();
                        GameObject pGO = p.TagObject as GameObject;
                        print("PGO is :" + pGO.gameObject.name);
                        string parentName = gameObject.GetComponentInParent<GameObject>().name;
                        if (pGO.gameObject.name == parentName) continue;
                        PlayerManager pPM = pGO.GetComponent<PlayerManager>();
                        if (pPM.amIOut)
                        {
                            pPM.ImBackIn();
                        }
                    }*/
                }

            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        catchSprite.SetActive(false);
    }
    

    
}
