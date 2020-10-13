using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator playerAnimationController; // players animation component
          
    [SerializeField]
    private float cameraRotationSpeed = 10;     // how fast does the camera rotate?

    [SerializeField]
    private GameObject CameraTarget;            // what does the thirdperson camera follow?

    // camera
    private Vector3 cameraRotation; // store camera rotation

    private void Awake()
    {
        playerAnimationController = gameObject.GetComponent<Animator>();    // get reference to players animation controller
    }


    void Start()
    {
        
    }

    // player movement
    public void Movement(Vector2 input)
    {
        playerAnimationController.SetFloat("WalkZ",input.y);            // trigger walk forward animation

        // horzontal rotation, in xz plane and around y axis
        transform.rotation = Quaternion.Euler(0, CameraTarget.transform.rotation.eulerAngles.y , 0);    // player rotates with the camera around y axis
    }

    //rotate the thirdperson camera
    public void RotateCamera(Vector2 lookDirection)
    {
        //since camera is using cinemachine to follow the target. rotating the target rotates the camera
        // rotate the camera target around its local y axis as a certain speed and direction per second
        // horzontal rotation, in xz plane and around y axis
        CameraTarget.transform.rotation *= Quaternion.AngleAxis(lookDirection.x * cameraRotationSpeed * Time.deltaTime, Vector3.up);
        // verticle rotation, in yz plane and around x axis
        CameraTarget.transform.rotation *= Quaternion.AngleAxis(lookDirection.y * cameraRotationSpeed * Time.deltaTime, Vector3.right);
        //camera rotation along x and y also causes some rotation in z axis
        cameraRotation = CameraTarget.transform.localEulerAngles; // store current camera local rotation 
        cameraRotation.z = 0; // eleminate the error in z rotation

        //Debug.Log(cameraRotation.x);
        // clamp camera rotation aorund x axis by desired amount
        if (cameraRotation.x > 50 && cameraRotation.x < 55)
        {
            cameraRotation.x = 50;
        }

        if (cameraRotation.x < 330 && cameraRotation.x > 55)
        {
            cameraRotation.x = 330;
        }

        // end clamp

        CameraTarget.transform.localEulerAngles = cameraRotation;   // reassign the rotation


    }
}
