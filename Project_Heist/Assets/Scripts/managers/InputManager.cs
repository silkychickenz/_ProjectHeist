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
    private Vector2 lookAround;

    [SerializeField]
    private int SetFPS = 120;

    // camera rotation effect on turning

    private void Awake()
    {
        // get reference
        Controls = new DefaultControls();
        playerControllerScript = Player.GetComponent<playerController>();
        

        // get input
        Controls.Player.move.performed += moveDirection => move = (moveDirection.ReadValue<Vector2>());
        Controls.Player.LookAround.performed += LookAround => lookAround = (LookAround.ReadValue<Vector2>());
       

        Application.targetFrameRate = SetFPS;
    }

    void Start()
    {
        LockCursor();   // lock and hide the cursor

    }


    void Update()
    {
        // call methods
        playerControllerScript.Movement(move);
        playerControllerScript.RotateCamera(lookAround);   // control the thirdperson camera




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
