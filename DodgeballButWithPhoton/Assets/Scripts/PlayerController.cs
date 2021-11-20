using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerManager pm;
    public bool viewIsMine = false;
    public CharacterController playerController;
    //public Transform playerTransform;
    public Camera cam;
    //public Camera camera;
    public float t;
    public float maxFOV;
    public float startFOV;
    public float speed;
    public float sprintSpeed;
    //private bool sprinting = false;
    public bool moving;
    public bool toggleSprint = true;
    public float mouseSensitivity = 100f;
    public float gravity = -9.14f;
    public float jumpHeight = 1.0f;
    private int jumpCount = 2;
    public float maxJumpHeight = 10f;
    public float minJumpHeight = 1f;
    public Vector3 playerVelocity;
    public float maxJumpVelocity;
    public float minJumpVelocity;
    public float timeToJumpApex = 0.4f;
    private bool groundedChar;

    public GameObject groundPoundObject;
    public Transform gpObjSpawn;
    //private bool moveGroundPound = false;
    public float dashLength;
    //public float turnSmoothTime = 0.1f;
    //float turnSmoothVelocity;
    public Vector3 move;
    private void Start()
    {
        viewIsMine = pm.photonView.IsMine;
        if (!viewIsMine)
        {
            Destroy(cam);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
    // Update is called once per frame
    void Update()
    {
        viewIsMine = pm.photonView.IsMine;
        if (viewIsMine)
        {
           // DoGroundPound();
            MoveCharacterXZ();
            MoveCharacterY();
        }
        
        //MoveCharacterSprint();
    }


    void MoveCharacterXZ()
    {
        groundedChar = playerController.isGrounded;
        if (groundedChar) jumpCount = 2;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0 || z != 0) moving = true;
        else moving = false;
        move = transform.right * x + transform.forward * z;
        playerController.Move(move * speed * Time.deltaTime);
    }

    void MoveCharacterY()
    {
        /*if (jumpCount > 0)
        {
            if (!groundedChar && Input.GetButtonDown("Jump"))
            {
                playerVelocity.y = Mathf.Sqrt(maxJumpVelocity * -3.0f * gravity);
            }
            if (Input.GetButtonUp("Jump"))
            {
                if (playerVelocity.y > minJumpVelocity)
                {
                    playerVelocity.y = minJumpVelocity;
                }
                jumpCount--;
            }
        }*/
        if (Input.GetButtonDown("Jump") && groundedChar)
        {
            playerVelocity.y = Mathf.Sqrt(maxJumpVelocity * -3.0f * gravity);
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (playerVelocity.y > minJumpVelocity)
            {
                playerVelocity.y = minJumpVelocity;
            }
        }
        //Debug.Log("Jumpcount is: " + jumpCount);
        playerVelocity.y += gravity * Time.deltaTime;

        playerController.Move(playerVelocity * Time.deltaTime);
        //end jump
    }

    void DoGroundPound()
    {
        if(!groundedChar)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                GameObject instance = Instantiate(groundPoundObject, gpObjSpawn.position, gpObjSpawn.rotation);
                groundPoundObject.transform.position = gpObjSpawn.position;
                //Instantiate(groundPoundObject);

                //Rigidbody rb = groundPoundObject.GetComponent<Rigidbody>();
                //Debug.Log(rb.velocity.y);
                instance.GetComponent<Rigidbody>().AddForce(-transform.up * 1000);
            }
        }
    }

    /*
    void MoveCharacterSprint()
    {
        //sprint
        //if player chooses to toggle (default) sprint
        if (toggleSprint)
        {
            if (groundedChar && Input.GetButtonDown("Sprint"))
            {
                Sprint();

            }
        }
        //if player chooses to hold sprint (toggle by default)
        if (!toggleSprint)
        {
            if (groundedChar && Input.GetButton("Sprint"))
            {
                Sprint();
            }
        }
        if (sprinting && Input.GetAxis("Horizontal") != 0)
        {
            speed /= sprintSpeed;
            sprinting = false;
            Camera.main.fieldOfView -= 5 * Time.deltaTime;
        }
        //print("horizontal axis" + Input.GetAxis("Horizontal"));
        if (sprinting)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, maxFOV, t);
        }
        if (!sprinting)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startFOV, t);
        }
    }
    void Sprint()
    {
        if (!sprinting)
        {
            speed *= sprintSpeed;
            sprinting = true;
            Camera.main.fieldOfView += 5 * Time.deltaTime;
        }
        else
        {
            speed /= sprintSpeed;
            sprinting = false;
            Camera.main.fieldOfView -= 5 * Time.deltaTime;
        }

    }
    */
}
