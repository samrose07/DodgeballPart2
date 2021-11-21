using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public byte team;
    private PhotonTeamsManager ptm;
    public MouseLook _camera;
    public int playersOutOnTeam1 = 0;
    public int playersOutOnTeam2 = 0;
    //public AudioClip ballHit;
    public AudioSource ballHitSource;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFiring);
            stream.SendNext(health);
            stream.SendNext(amIOut);
            stream.SendNext(playersOutOnTeam1);
            stream.SendNext(playersOutOnTeam2);
        }
        else
        {
            this.isFiring = (bool)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
            this.amIOut = (bool)stream.ReceiveNext();
            this.playersOutOnTeam1 = (int)stream.ReceiveNext();
            this.playersOutOnTeam2 = (int)stream.ReceiveNext();
        }
    }
    public SendRPClol rpcSendScript;
    public GameObject ball;
    bool isFiring;
    public float health = 1f;
    public static GameObject localPlayerInstance;
    public bool amIOut = false;

    private void Awake()
    {
        if(ball == null)
        {
            Debug.Log("Missing ball reference", this);
        }
        else
        {
            ball.SetActive(true);
        }
        if (photonView.IsMine)
        {
            photonView.Owner.TagObject = gameObject;
            PlayerManager.localPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //CameraWork _camera = this.gameObject.GetComponent<CameraWork>();
       // MouseLook _camera = this.gameObject.GetComponentInChildren<MouseLook>();

        if(_camera != null)
        {
            if (photonView.IsMine)
            {
                _camera.isViewMine = true;

            }
            else
            {
                Debug.Log("No camera in scene");
            }
        }
        ptm = GameManager.instance.GetComponent<PhotonTeamsManager>();
    }
    private void Update()
    {
        if(ball != null && isFiring != ball.activeInHierarchy)
        {
            ball.SetActive(isFiring);
        }
        if (photonView.IsMine)
        {
            ProcessInputs();
            if (health <= 0f)
            {
                GameManager.instance.LeaveRoom();
            }
            if(team != 0)
            {
                if (PhotonNetwork.LocalPlayer.GetPhotonTeam() != null) PhotonNetwork.LocalPlayer.SwitchTeam(team);
                else PhotonNetwork.LocalPlayer.JoinTeam(team);
                //Photon.Realtime.Player[] players;
                //PhotonNetwork.LocalPlayer.TryGetTeamMates(out players);
                //print(players.Length);
            }
            if (amIOut) StayOut();
            GameManager.instance.playersOutTeam1 = playersOutOnTeam1;
            GameManager.instance.playersOutTeam2 = playersOutOnTeam2;
            

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!other.name.Contains("Ball"))
        {
            return;
        }
        health -= 0.1f;
    }

    public void UpdatePlayersOut(bool add, int team)
    {
        //print("update players out");
        if (add)
        {
            if (team == 1)
            {
                playersOutOnTeam1++;
            }
            else
            {
                playersOutOnTeam2++;
            }
        }
        else
        {
            if(team == 1)
            {
                playersOutOnTeam1--;
            }
            else
            {
                playersOutOnTeam2--;
            }
        }
    }

    public void ImOut()
    {
       // Debug.Log(gameObject.name + " was hit by ");
        //TODO: MOVE THE GAME OBJECT TO OUT AREA
        ballHitSource.Play();
        amIOut = true;
        //print(this.gameObject.name + " in playerManager ImOut");
        rpcSendScript.SendTeamOutUpdate("SendTeamOut", this.team, this.gameObject.name, true);
        StayOut();
        
    }

    void StayOut()
    {
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.enabled = false;
        this.gameObject.transform.position = new Vector3(3.6f, 1.54f, -10.7f);
    }

    public void ImBackIn()
    {
        Debug.Log("I am now back in");
        amIOut = false;
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.enabled = true;
        rpcSendScript.SendTeamOutUpdate("SendTeamOut", this.team, this.gameObject.name, false);
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                isFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (isFiring)
            {
                isFiring = false;
            }
        }

        
    }


}
