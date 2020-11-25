using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private DefaultControls Controls;      // input asset

    [Header("Get references")]
    [SerializeField]
    private GameObject Player;

    //movement
    private playerController playerControllerScript;

   
    //store movement input
    private Vector2 move;
    //store camera input
    private Vector2 lookAround;
   

    [Header("Get references")]
    //store movement input and gravity flip mechanic variables
    private Vector2 gravityFlipDirection;
    [SerializeField]
    private float GravityFlipCooldownTimer = 1; // gravity flip cooldown in seconds
    [SerializeField]
    public bool enableGravityFlip = true; // can you flip gravity?


    [SerializeField]
    private int SetFPS = 120;

    //TEMP
    private float TEMPcam;

    private void Awake()
    {
        // get reference
        Controls = new DefaultControls();
        playerControllerScript = Player.GetComponent<playerController>();
        

        // get input
        Controls.Player.move.performed += moveDirection => move = (moveDirection.ReadValue<Vector2>());
        Controls.Player.LookAround.performed += LookAround => lookAround = (LookAround.ReadValue<Vector2>());
        Controls.Player.flipGravityPlayer.performed += flipGravity => gravityFlipDirection = (flipGravity.ReadValue<Vector2>());

        Application.targetFrameRate = SetFPS;

        //TEMP
        Controls.Player.TEMPcameratoggle.performed += TEMPCam => TEMPcam = (TEMPCam.ReadValue<float>());
        
    }

    void Start()
    {
        //LockCursor();   // lock and hide the cursor

       
    }


    void Update()
    {
        // call methods
        playerControllerScript.Movement(move);
        playerControllerScript.PlayerRotation(lookAround, move);
        playerControllerScript.Gravity();
        playerControllerScript.PlayerAlwaysUpright();

        /*
        //GRAVITY FLIP
        playerControllerScript.GravityFlip(gravityFlipDirection, enableGravityFlip);
        if (gravityFlipDirection != Vector2.zero && enableGravityFlip) //if there is gravity flip input and graty was freviously flipped
        {
            enableGravityFlip = false;
            StartCoroutine(GravityFlipCooldown());
        }
        
          */


        if (TEMPcam >0)  //TEMP
        {
            playerControllerScript.RotateCamera(lookAround);   // control the thirdperson camera
        }
        




    }

    private void FixedUpdate()
    {
        //GRAVITY FLIP
        playerControllerScript.GravityFlip(gravityFlipDirection, enableGravityFlip);
        if (gravityFlipDirection != Vector2.zero && enableGravityFlip) //if there is gravity flip input and graty was freviously flipped
        {
            enableGravityFlip = false;
            StartCoroutine(GravityFlipCooldown());
        }
    }



    // lock and hide the cursor in game 



    public IEnumerator GravityFlipCooldown() // fravity flip cooldown timer
    {
        yield return new WaitForSeconds(GravityFlipCooldownTimer);
        enableGravityFlip = true;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
