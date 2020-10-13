using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private DefaultControls Controls;      // input asset

    [Header("Get references")]
    [SerializeField]                // player
    private GameObject Player;          //player reference
    private Player PlayerScript;        // player movement script

    [SerializeField]                // camera
    private GameObject CameraTarget;            // get the game object camera is following
    private SimpleFollowScript followScript;    //sinple follow script

    // get input
    private Vector2 input;               // movememt imput
    private Vector2 lookDirection;       // camera input

    

    private void Awake()
    {
        // get reference
        Controls = new DefaultControls();     // create instance for input 

        // script references *****************
        PlayerScript = Player.GetComponent<Player>();       //  script attached to player
        followScript = CameraTarget.GetComponent<SimpleFollowScript>(); // a simple follow script atached to camera target


        // End script references *****************

        // inetialize input ************************
        Controls.Player.Move.performed += ThereIsMoveInput => input = (ThereIsMoveInput.ReadValue<Vector2>());
        Controls.Player.lookAround.performed += ThereIsCameraRotationInput => lookDirection = (ThereIsCameraRotationInput.ReadValue<Vector2>());
        // End inetialize input ************************

    }



    void Start()
    {
        LockCursor();   // lock and hide the cursor
    }

   
    void Update()
    {
        PlayerScript.Movement(input);   // move the player
        PlayerScript.RotateCamera(lookDirection);   // control the thirdperson camera
        followScript.follow();  // camera target follows the player
    }

    // lock and hide the cursor in game 
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
