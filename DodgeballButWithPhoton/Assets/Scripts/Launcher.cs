using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public byte maxPlayersPerRoom = 4;
    public GameObject uiPanel;
    public GameObject progressLabel;
    public int timer;
    string gameVersion = "1";
    bool isConnecting;
    public bool canTime = false;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void setCanTime()
    {
        canTime = true;
    }
    private void Start()
    {
        progressLabel.SetActive(false);
        uiPanel.SetActive(true);

    }

    public void Connect()
    {

            progressLabel.SetActive(true);
            uiPanel.SetActive(false);
            if (PhotonNetwork.IsConnected) PhotonNetwork.JoinRandomRoom();
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
       
    }
    public void QuitApp()
    {
        Application.Quit();
    }
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });

    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        uiPanel.SetActive(true);
        isConnecting = false;
    }
}
