using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Tooltip("Speed at which the camera rotates around the player. (Sensitivity, as it's named in most games)")]
    public float rotSpeed = 1;

    [Tooltip("Speed at which the player Lerps rotation to camera forward")]
    public float time = 0.5f;

    public Transform target, player;
    [Tooltip("Player Controller attached to the dude")]
    public PlayerController playerController;
    bool moving;
    float mouseX, mouseY;
    public bool isViewMine = false;
    public Camera theCam;
    public GameObject theCanvas;
    //public PlayerManager pm;
    private void Start()
    {
        if (isViewMine)
        {
            theCanvas.SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        if (isViewMine)
        {
            //theCam = Camera.main;
            // theCam.transform.parent = target.transform;
            CheckEscape();
            CamControl();
            moving = playerController.moving;
        }
        else
        {
            Destroy(theCanvas);
        }
        
    }
    void CheckEscape()
    {
        if (theCanvas.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                theCanvas.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                theCanvas.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        
    }

    public void LeaveRoom()
    {
        GameManager.instance.LeaveRoom();
    }
    void CamControl()
    {

        mouseX += Input.GetAxis("Mouse X") * rotSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        //theCam.transform.position = target.transform.position + new Vector3(mouseX, mouseY,0);
        transform.LookAt(target);

        if (!moving)
        {
            target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
        else
        {
            target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, mouseX, 0), time);
            //player.rotation = Quaternion.Euler(0, mouseX, 0);
        }
        //Debug.Log("Camera's moving is: " + moving);

    }
    /*public PlayerController playerController;
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public bool enableCameraMovement = true;
    public bool lockAndHideCursor = true;

    //new attempt
    public float mouseXMult = 50f;
    public float turnSpeed = 4.0f;
    public Transform player;

    public float height = 1f;
    public float dist = 2f;

    private Vector3 offsetX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        offsetX = new Vector3(0, height, dist);


    }

    // Update is called once per frame
    void Update()
    {

        if (enableCameraMovement)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerBody.Rotate(new Vector3(0, 1, 0) * (mouseX * mouseXMult));

            
        }
        else return;
    }
    private void LateUpdate()
    {
        offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetX;
        
        transform.position = player.position - offsetX;
        transform.LookAt(player.position);
    }*/
}
