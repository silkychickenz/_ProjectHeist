using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
//using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private DefaultControls Controls;      // input asset

    [Header("Get references")]
    [SerializeField]
    private GameObject Player,playerAvatar;
    private Animator animator;
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
    public bool canShoot = true;
    //gravity flip input
    private Vector2 gravityFlipDirection;
    public bool enableGravityFlip = true; // can you flip gravity?
    private bool gravityFlipWheel = false;
    // jump input
    private bool Jump = false;
    

    [Header("Gravity")]
    [SerializeField]
    private float GravityFlipCooldownTimer = 1, weaponROF = 1; // gravity flip cooldown in seconds

    // cover input
    private bool takeCover = false;

    private bool isCoverDetected;



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
        Controls.Player.Shooting.performed += StartShooting => startShooting = !startShooting;
       

        // get jump input
        Controls.Player.Jump.performed += StartJumping => Jump = true;
        Controls.Player.Jump.canceled += StartJumping => Jump = false;

        // get jump input
        Controls.Player.cover.performed += TakeCover => takeCover = !takeCover;
        

        Application.targetFrameRate = SetFPS;

        //TEMP
        Controls.Player.TEMPcameratoggle.performed += TEMPCam => TEMPcam = (TEMPCam.ReadValue<float>());
        
    }

    void Start()
    {
        LockCursor();   // lock and hide the cursor
        canShoot = true;
        animator = playerAvatar.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (TEMPcam == 1)
        {
            SceneManager.LoadScene(0);
        }
       
        if (takeCover && !playerControllerScript.isCoverDetected)
        {
            playerControllerScript.CoverDetection();
        }
        if (!playerControllerScript.isCoverDetected)
        {
            takeCover = false;
        }

        playerControllerScript.playerFalling(move);  
        playerControllerScript.Movement(move, Jump, startSprinting, startCrouching, startAiming, crouchBoost, takeCover);
        if (!startCrouching && !startAiming)
        {
            playerControllerScript.JumpAndVault(Jump, move, startSprinting);
        }
        
        Jump = false;
        gravityScipt.ApplyGravity();
       
        gravityScipt.GravityFlip(gravityFlipDirection, enableGravityFlip, gravityFlipWheel);
        if (gravityFlipDirection != Vector2.zero && enableGravityFlip) //if there is gravity flip input and graty was freviously flipped
        {

            enableGravityFlip = false;
            StartCoroutine(GravityFlipCooldown());
        }



    }

    void Update()
    {
       
        playerControllerScript.JumpAndVaultAnimation();
        if (!startAiming) //if player is not aiming
        {
           
            playerControllerScript.animator.SetBool("StartShooting", false); // get out of shooting mode
            playerControllerScript.Hitscan(false); // stop firing bullets
        }
        if (!startCrouching)
        {
            playerControllerScript.MovementAnimation(move, Jump, startSprinting, startAiming);
           
            crouchBoost = false;

        }
        if (startCrouching)
        {
            playerControllerScript.CrouchMovementAnimation(move, startCrouching, crouchBoost, startAiming);
            crouchBoost = true;
        }

        if (startAiming)
        {

          
            playerControllerScript.ShootingMovementAnimation(move, startAiming, startShooting, canShoot); // enter shooting movement mode
            if (gravityScipt.isPlayerGrounded)
            {
                
                playerControllerScript.isPlayerGrounded = true;
            }
            if (startShooting)
            {
                if (canShoot)
                {
                    playerControllerScript.Hitscan(startShooting); // start firing

                    canShoot = false;
                    StartCoroutine(WeaponROF());
                }

            }
      

        }

       
        
       
        playerControllerScript.CoverAnimations(takeCover);

        



    }

   

    private void LateUpdate()
    {
        playerControllerScript.PlayerAlwaysUpright(move);
        playerControllerScript.RotateCamera(lookAround);
    }


    public IEnumerator GravityFlipCooldown() // fravity flip cooldown timer
    {
        yield return new WaitForSeconds(GravityFlipCooldownTimer);
        enableGravityFlip = true;
    }

    public IEnumerator WeaponROF() // fravity flip cooldown timer
    {
        yield return new WaitForSeconds(weaponROF);
        animator.SetLayerWeight(3, 0);
        canShoot = true;
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
