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
    private crouchMovement crouchMovementScript;
    private bool crouchBoost = true;

    //shooting
    private Shooting ShootingScript;
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
    private bool Jump;

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
        crouchMovementScript = Player.GetComponent<crouchMovement>();


        ShootingScript = Player.GetComponent<Shooting>();
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
        Controls.Player.Jump.performed += StartJumping => Jump = true;
       

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


       playerControllerScript.playerFalling(move);

        if (!startAiming) //if player is not aiming
        {
            if (!startCrouching)
            {
                playerControllerScript.Movement(move, Jump, startSprinting);
                Jump = false;
                crouchBoost = true;
                
            }
            if (startCrouching)
            {
                crouchMovementScript.CrouchMovement(move,startCrouching, crouchBoost);
                crouchBoost = false;
            }
           

            ShootingScript.animator.SetBool("StartShooting", false); // get out of shooting mode
          
            ShootingScript.Hitscan(false); // stop firing bullets
        }
        
        playerControllerScript.PlayerAlwaysUpright();
      
        gravityScipt.ApplyGravity();


        if (startAiming)
        {
            ShootingScript.Hitscan(startShooting); // start firing
            ShootingScript.ShootingMovement(move, startAiming); // enter shooting movement mode
            
            
           
            if (gravityScipt.isPlayerGrounded)
            {
                
                playerControllerScript.isPlayerGrounded = true;
            }



        }
        
       


        if (TEMPcam <= 0) //hold C to lock the camera
        //if (TEMPcam >0)  //hold C to unlock the camera
        {
           playerControllerScript.RotateCamera(lookAround);   // control the thirdperson camera
           
        }


       


    }

    private void FixedUpdate()
    {
        //GRAVITY FLIP
        //playerControllerScript.GravityFlip(gravityFlipDirection, enableGravityFlip);
        
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
