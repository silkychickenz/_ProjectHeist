using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Get references")]
    [SerializeField]
    private Animator animator;
    private Rigidbody rb;



    [Header("Camera")]
    [SerializeField]
    private GameObject CameraTarget;
    [SerializeField]
    private float cameraRotationSpeed = 10;     // how fast does the camera rotate?
    [SerializeField]
    private float playerRotatingWithCameraSpeed = 10;     // how fast does the camera rotate?

    [Header("player Rotation")]
    [SerializeField]
    private float playerRotationSpeed = 15;

    [Header("Gravity Flipping")]
    [SerializeField]
    private LayerMask Walkable; // what layer can the player walk on
    [SerializeField]
    private float RayCastLength = 10; // how far to cast the ray?
    private Vector3 raycastDirection;

    [Header("player gravity")]
    [SerializeField]
    private float gravity;
    [SerializeField]
    private bool isPlayerGrounded =  true;
    [SerializeField]
    private float groundCheckDist = 0.2f;





    // camera system
    private Vector3 cameraRotation; // store camera rotation


    // player rotation system
    private Vector3 inputDirection; // vector 3 version of the directional input
    private float playerRotatingDirection;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // player walk/run
    public void Movement(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            animator.SetBool("startWalking" , true);
        }
        else if (direction == Vector2.zero)
        {
            animator.SetBool("startWalking", false);
        }
    }

    // player turning / change direction
    public void PlayerRotation(Vector2 lookDirection , Vector2 direction)
    {
        Debug.DrawRay(transform.position, inputDirection  * 5, Color.green); // input direction
        Debug.DrawRay(CameraTarget.transform.position, CameraTarget.transform.forward * 5, Color.yellow);
        Debug.DrawRay(CameraTarget.transform.position, CameraTarget.transform.right * 5, Color.black);

        inputDirection = (direction.x * CameraTarget.transform.right + direction.y * CameraTarget.transform.forward);
        


        //player roation when there is movement input
        if ( direction != Vector2.zero)
        {
            playerRotatingDirection = Mathf.Sign(Vector3.Dot(inputDirection.normalized, gameObject.transform.right));
           // Debug.Log(playerRotatingDirection);
            if (Vector3.Angle(gameObject.transform.forward, inputDirection) !=0 && Mathf.Abs(Vector3.Dot(inputDirection.normalized, gameObject.transform.right)) > 0.08)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
            }
        }


       


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

    public void GravityFlip(Vector2 direction)
    {
        raycastDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.up);
       
        RaycastHit hitinfo;
        Debug.DrawRay(gameObject.transform.position, raycastDirection * RayCastLength, Color.red); // ground check ray visualized
        if (Physics.Raycast(gameObject.transform.position, raycastDirection, out hitinfo, RayCastLength, Walkable))
        {
            Debug.Log("detected" + hitinfo.collider.gameObject.name);
            
        }
        
        if (direction != Vector2.zero)
        {
            animator.SetBool("gravityFlip", true);
        }
        else if (direction == Vector2.zero)
        {
            animator.SetBool("gravityFlip", false);
        }
    }

    public void Gravity(float currentGravity = 0)
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hitinfo, groundCheckDist, Walkable))
        {

            isPlayerGrounded = true;

        }

        else
        {
            isPlayerGrounded = false;
        }



        if (!isPlayerGrounded)
        {
            currentGravity += (gravity * Time.deltaTime);

            rb.AddForce(currentGravity * -gameObject.transform.up, ForceMode.Force);
        }

        else if (isPlayerGrounded)
        {
            currentGravity = 0;
        }
        
    }

}

