using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public GameObject player;
    public bool startGameTrigger = false;
    public int playersInRoom;
    public int playersOutTeam1 = 0;
    public int playersOutTeam2 = 0;
    private void Start()
    {
        instance = this;
        if (player != null)
        {
            if(PlayerManager.localPlayerInstance == null)
            {
                PhotonNetwork.Instantiate(this.player.name, new Vector3(0f, 10f, 0f), Quaternion.identity);
                PlayerManager.localPlayerInstance.gameObject.name = PhotonNetwork.NickName;
            }
            
        }
    }

    private void Update()
    {
        if (PlayerManager.localPlayerInstance != null && PlayerManager.localPlayerInstance.transform.position.y < -50)
        {
            Vector3 newPos = new Vector3(PlayerManager.localPlayerInstance.transform.position.x, 10, PlayerManager.localPlayerInstance.transform.position.z);
            PlayerManager.localPlayerInstance.transform.position = newPos;
        }
        Scene activeScene;
        activeScene = SceneManager.GetActiveScene();
        string curScene = activeScene.name;
        if(curScene == "TeamSelect")
        {
            if (PhotonNetwork.CurrentRoom != null)playersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;
            //changed for testing
            if (playersInRoom == 4)
            {
                Player[] membersOnTeam1;
                Player[] membersOnTeam2;
                PhotonTeamsManager.Instance.TryGetTeamMembers(1, out membersOnTeam1);
                PhotonTeamsManager.Instance.TryGetTeamMembers(2, out membersOnTeam2);

                if (membersOnTeam2.Length == 2 && membersOnTeam1.Length == 2)
                {
                    StartFromTeamSelect();
                }
            }
        }

        if(curScene == "Dodgeball")
        {
            if (playersOutTeam1 == 2)
            {
                //todo ADD THE WIN CONDITION BASED ON TEAM;
                SceneManager.LoadScene("WinSceneBananaSlugs");
            }
            if (playersOutTeam2 == 2)
            {
                //todo ADD THE WIN CONDITION BASED ON TEAM
                //print("players out team 2 game manager");
                SceneManager.LoadScene("WinSceneRichards");
            }
        }

        if(curScene == "SampleScene")
        {
            Destroy(PlayerManager.localPlayerInstance);
        }
    }

    public void StartFromTeamSelect()
    {
        SceneManager.LoadScene("Dodgeball");
        
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArena("TeamSelect");
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
           
            LoadArena("TeamSelect");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena(string level)
    {
        PhotonNetwork.LoadLevel(level);
    }
    
}
