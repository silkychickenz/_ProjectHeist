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
    [SerializeField]
    private Vector3 raycastDirection;
    [SerializeField]
    private float GravityFlipStartForce = 8; // when player uses gracity flip, a jump for is applied that pushed the player off the ground?
    [SerializeField]
    private float flipRotationSpeed = 10; // how fast does the pl;ayer rotate when gravity is flipped
    private float currentRotationTracker = 0;
    [SerializeField]
    private float MinDistanceBeforeFlip = 2; // how high should player jump before a backflip?
    private bool isEnoughDistFromGround = false;
    private bool tempGravityDisabler = false; // disable the gravity temporaroly?
    private  float RotByDegrees = 0;
    private bool Rotating = false;



    [Header("player gravity")]
    [SerializeField]
    private float gravity = 19; 
    [SerializeField]
    private bool isPlayerGrounded =  true;
    [SerializeField]
    private float groundCheckDist = 0.2f;
    private float currentGravity = 0;
   


    [Header("player rotation recovery")]
    [SerializeField]
    private float PlayerRecoverySpeed = 10;
    [SerializeField]
    float SphereCastRadius = 2;
    [SerializeField]
    float SphereCastDistance = 2f;
    [SerializeField]
    private GameObject RotRecoveryCheckPos;
    [SerializeField]
    private LayerMask sphereCastDetectable;






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
        if (direction != Vector2.zero && isPlayerGrounded)
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

    //gravity flip mechanic
    public void GravityFlip(Vector2 direction, bool CanFlipGravity) // direction gets the input and CanFlipGravity gets the cooldown, 
    {
        
        raycastDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.up); // direction of raycast
       
        // raycast in the direction player wants to flip
        RaycastHit hitinfo;
        Debug.DrawRay(gameObject.transform.position, raycastDirection * RayCastLength, Color.red); // ground check ray visualized
        Physics.Raycast(gameObject.transform.position, raycastDirection, out hitinfo, RayCastLength, Walkable);
        
        // only responsible for adding force and starting the grlip
        if (raycastDirection != Vector3.zero  && CanFlipGravity && !isEnoughDistFromGround) // if there is input and its not on cooldowm
        {
            
            // currentGravity = 0;
            rb.AddForce(GravityFlipStartForce * gameObject.transform.up, ForceMode.Impulse); // boost the player a little off the ground
           
            Rotating = true;
            
        }

        if (direction.y == 1)
        {
            RotByDegrees = -180;
        }

        if (direction.x == 1)
        {
            RotByDegrees = 90;
        }

        if (direction.x == -1)
        {
            RotByDegrees = -90;
        }

        //ROTATION when gravity flipped
        // pressing up will alwasy have 180 rotation in players x axis

        //raycast to check if player is minimum distance from ground to begin flipping
        RaycastHit hitinfoDistance;
        Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hitinfoDistance, Mathf.Infinity, Walkable);
        if (hitinfoDistance.distance > MinDistanceBeforeFlip)
        {
            isEnoughDistFromGround = true;
        }
        if ( Mathf.Abs(currentRotationTracker) >= Mathf.Abs(RotByDegrees) && hitinfoDistance.distance <= MinDistanceBeforeFlip) // if the player is successfully completed a rotation and is withing minimum distance for flip
        {
            isEnoughDistFromGround = false;
        }


        
       // Debug.Log("isEnoughDistFromGround : " + isEnoughDistFromGround);
        
       // Debug.Log("RotByDegrees : " + RotByDegrees);
        //if (Mathf.Abs(currentRotationTracker) <= RotByDegrees && isEnoughDistFromGround)
        if (Rotating  && isEnoughDistFromGround)
        {
            tempGravityDisabler = true;
            //Debug.Log("currentRotationTracker : " + currentRotationTracker);
            if (Mathf.Abs(RotByDegrees) == 180)
            {
                //transform.Rotate(new Vector3(RotByDegrees * flipRotationSpeed * Time.deltaTime, 0, 0), Space.Self);
                transform.Rotate(new Vector3(0, 0, RotByDegrees * flipRotationSpeed * Time.deltaTime), Space.Self);
            }

            if (Mathf.Abs(RotByDegrees) == 90)
            {
                transform.Rotate(new Vector3(0, 0, RotByDegrees * flipRotationSpeed * Time.deltaTime), Space.Self);
            }

            if (Mathf.Abs(RotByDegrees) == -90)
            {
                transform.Rotate(new Vector3(0, 0, -RotByDegrees * flipRotationSpeed * Time.deltaTime), Space.Self);
            }

            currentRotationTracker += (RotByDegrees * flipRotationSpeed * Time.deltaTime);

            if (Mathf.Abs(currentRotationTracker) >= Mathf.Abs(RotByDegrees) -10 && currentRotationTracker <= Mathf.Abs(RotByDegrees) + 10)
            {
                if (Mathf.Abs(RotByDegrees) == 180)
                {
                    //gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
                    if (Mathf.Abs(gameObject.transform.eulerAngles.z) <= 190 && Mathf.Abs(gameObject.transform.eulerAngles.z) >= 170)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180);
                    }

                    else if (Mathf.Abs(gameObject.transform.eulerAngles.z) <= 10)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y,0);
                    }

                    else if (Mathf.Abs(gameObject.transform.eulerAngles.z) <= 280 && Mathf.Abs(gameObject.transform.eulerAngles.z) >= 260)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 270);
                    }

                    else if (Mathf.Abs(gameObject.transform.eulerAngles.z) <= 100 && Mathf.Abs(gameObject.transform.eulerAngles.z) >= 80)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
                    }

                }
                if (Mathf.Abs(RotByDegrees) == 90)
                {

                    if (gameObject.transform.eulerAngles.z <= 10 )
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
                       // gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);

                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 100 && (gameObject.transform.eulerAngles.z) >= 80)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
                        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 0, 90);
                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 190 && (gameObject.transform.eulerAngles.z) >= 170)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180);
                        // gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 180);
                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 280 && (gameObject.transform.eulerAngles.z) >= 260)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 270);
                       // gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 0, 270);
                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 370 && (gameObject.transform.eulerAngles.z) >= 350)
                    {
                         gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 360);
                         //gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 360);
                    }
                }

                //currentGravity = 0;
                // Debug.Log(RotByDegrees);
                //Debug.Log((gameObject.transform.eulerAngles.z));
                tempGravityDisabler = false;
                Rotating = false;
                currentRotationTracker = 0;
                RotByDegrees = 0;
            }

        }


        //Debug.Log(currentRotationTracker);
        //Debug.Log((gameObject.transform.eulerAngles.z));


    }

    public void PlayerAlwaysUpright()
    {
        //Debug.Log("NORMAL : " + Vector3.Angle(gameObject.transform.up, hitinfo.normal ));
     

        RaycastHit SphereCastInfo;
        Physics.SphereCast(RotRecoveryCheckPos.transform.position, SphereCastRadius, -gameObject.transform.up,out SphereCastInfo, SphereCastDistance, sphereCastDetectable, QueryTriggerInteraction.UseGlobal );
        Debug.DrawRay(RotRecoveryCheckPos.transform.position, -gameObject.transform.up * SphereCastDistance, Color.magenta);
        Debug.Log("DOT : " + Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal));
       // Debug.Log((gameObject.transform.eulerAngles.x));
       // Debug.Log(SphereCastInfo.collider.gameObject.name);
        if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) >= 1 )
        {
            if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) < -0.1)
            {
                transform.Rotate(new Vector3(-PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
            }
            else if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) > 0.1)
            {
                transform.Rotate(new Vector3(PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
            }
        }
    }

    // apply gravity
    public void Gravity()
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

        //Debug.Log(currentGravity);
       
        if (!isPlayerGrounded && !tempGravityDisabler && currentGravity <= gravity)
        {
            currentGravity += (gravity * Time.deltaTime);

            rb.AddForce(currentGravity * -gameObject.transform.up, ForceMode.Force);
        }

        else if (isPlayerGrounded)
        {
            currentGravity = 0;
        }

        if (currentGravity > gravity)
        {
            currentGravity = gravity;
        }
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(RotRecoveryCheckPos.transform.position + -gameObject.transform.up * SphereCastDistance, SphereCastRadius);
    }

}

