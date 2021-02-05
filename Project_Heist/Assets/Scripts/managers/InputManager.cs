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
    
    private bool crouchBoost = true;

    
    //gravity
    private Gravity gravityScipt;

    //store movement input
    private Vector2 move;
    private bool startSprinting = false;
    private bool startCrouching = false;
    //store camera input
    private Vector2 lookAround;
    // shooting input
    private bool startAiming;
    private bool startShooting;
    //gravity flip input
    private Vector2 gravityFlipDirection;
    public bool enableGravityFlip = true; // can you flip gravity?
    private bool gravityFlipWheel = false;
    // jump input
    private bool Jump = false;

    [Header("Gravity")]
    [SerializeField]
    private float GravityFlipCooldownTimer = 1; // gravity flip cooldown in seconds

    


    [SerializeField]
    private int SetFPS = 120;

    //TEMP
    private float TEMPcam;

    private void Awake()
    {
        // get reference
        Controls = new DefaultControls();
        playerControllerScript = Player.GetComponent<playerController>();
        


       
        gravityScipt = Player.GetComponent<Gravity>();


        // get Movement input
        Controls.Player.move.performed += moveDirection => move = (moveDirection.ReadValue<Vector2>());
        Controls.Player.Sprint.performed += sprintToggle => startSprinting = !startSprinting;
        Controls.Player.LookAround.performed += LookAround => lookAround = (LookAround.ReadValue<Vector2>());
        Controls.Player.crouch.performed += crouchToggle => startCrouching = !startCrouching;

        // get gravity input
        Controls.Player.GravityFlipWheel.performed += GravityFlipWheelToggle => gravityFlipWheel = !gravityFlipWheel;
        Controls.Player.flipGravityPlayer.performed += flipGravity => gravityFlipDirection = (flipGravity.ReadValue<Vector2>());

        // get shooting input
        Controls.Player.Aiming.performed += StartShooting => startAiming = true;
        Controls.Player.Aiming.canceled += StartShooting => startAiming = false;
        Controls.Player.Shooting.performed += StartShooting => startShooting = true;
        Controls.Player.Shooting.canceled += StartShooting => startShooting = false;

        // get jump input
        Controls.Player.Jump.performed += StartJumping => Jump = !Jump;
       

        Application.targetFrameRate = SetFPS;

        //TEMP
        Controls.Player.TEMPcameratoggle.performed += TEMPCam => TEMPcam = (TEMPCam.ReadValue<float>());
        
    }

    void Start()
    {
        LockCursor();   // lock and hide the cursor
        
    }


    void Update()
    {

        if (!startAiming) //if player is not aiming
        {
            if (!startCrouching)
            {
                playerControllerScript.MovementAnimation(move, Jump, startSprinting);
              
                crouchBoost = false;
                
            }
            if (startCrouching)
            {
                playerControllerScript.CrouchMovementAnimation(move,startCrouching, crouchBoost);
                crouchBoost = true;
            }

            playerControllerScript.animator.SetBool("StartShooting", false); // get out of shooting mode
            playerControllerScript.Hitscan(false); // stop firing bullets
        }

        if (startAiming)
        {
            playerControllerScript.Hitscan(startShooting); // start firing
            playerControllerScript.ShootingMovementAnimation(move, startAiming); // enter shooting movement mode
            if (gravityScipt.isPlayerGrounded)
            {
                
                playerControllerScript.isPlayerGrounded = true;
            }


        }
 
    }

    private void FixedUpdate()
    {

        playerControllerScript.playerFalling(move);
        playerControllerScript.PlayerAlwaysUpright();
        playerControllerScript.Movement(move, Jump, startSprinting, startCrouching, startAiming, crouchBoost);
        gravityScipt.ApplyGravity();
        playerControllerScript.RotateCamera(lookAround);
        gravityScipt.GravityFlip(gravityFlipDirection, enableGravityFlip, gravityFlipWheel);
            if (gravityFlipDirection != Vector2.zero && enableGravityFlip) //if there is gravity flip input and graty was freviously flipped
            {
                enableGravityFlip = false;
                StartCoroutine(GravityFlipCooldown());
            }
        
        

    }


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
        //Controls.Disable();
    }
}
