using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    
    private Animator animator;
    private Rigidbody rb;
    [SerializeField]
    private GameObject playerAvatar;

    [Header("Camera")]
    [SerializeField]
    private GameObject CameraTarget;
    [SerializeField]
    private float cameraRotationSpeed = 10;     // how fast does the camera rotate?
    [SerializeField]
    private float playerRotatingWithCameraSpeed = 10;     // how fast does the camera rotate?


    [Header("player rotation recovery")]
    [SerializeField]
    private float PlayerRecoverySpeed = 10;
    [SerializeField]
    float SphereCastRadius = 2;
    [SerializeField]
    float SphereCastDistance = 2f;
    [SerializeField]
    private GameObject playerMidPoint;
    [SerializeField]
    private LayerMask sphereCastDetectable;
    Gravity gravityScript;
    RaycastHit SphereCastInfo;

    // camera system
    private Vector3 cameraRotation; // store camera rotation


    // player rotation system
    private Vector3 inputDirection; // vector 3 version of the directional input
    private float playerRotatingDirection;

    [Header(" new player movement")]
    [SerializeField]
    private float playerRotationSpeed = 15;
    [SerializeField]
    private LayerMask Walkable; // what layer can the player walk on
    [SerializeField]
    public bool isPlayerGrounded = true;
    [SerializeField]
    private float groundCheckDist = 0.2f;
    [SerializeField]
    private float MaxPlayerWalkSpeed = 5, MaxPlayerRunSpeed = 10, jumpForce = 10;
    private float MovementForce = 0;
    private Vector3 moveForceDirection;
    float currentInPutDotProduct;
    Vector3 groundPlayerCross;

    [Header(" PLAYER FALLING")]
    [SerializeField]
    private float fallCheckDistance = 15;
    [SerializeField]
    float airSpeed = 5;
    private Vector3 airInputDirection;

    void Start()
    {
        animator = playerAvatar.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        gravityScript = gameObject.GetComponent<Gravity>();
        moveForceDirection = gameObject.transform.forward;
    }

    // player walk/run
    public void newMovement(Vector2 direction, bool jump, bool startSprint)
    {
        playerAvatar.transform.localPosition = Vector3.zero;

        Debug.DrawRay(gameObject.transform.position, Vector3.Cross(gameObject.transform.right, SphereCastInfo.normal) * 4, Color.green);
        isPlayerGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, Walkable);
       
        #region playerortation
        //player roation when there is movement input

        inputDirection = (direction.x * CameraTarget.transform.right + direction.y * CameraTarget.transform.forward);
        currentInPutDotProduct = Vector3.Dot(gameObject.transform.forward, inputDirection);
       
        if (direction != Vector2.zero && isPlayerGrounded)
        {
            playerRotatingDirection = Mathf.Sign(Vector3.Dot(inputDirection.normalized, gameObject.transform.right));

            if (Vector3.Angle(gameObject.transform.forward, inputDirection) != 0 && Mathf.Abs(Vector3.Dot(inputDirection.normalized, gameObject.transform.right)) > 0.08)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
            }
            if (Vector3.Angle(gameObject.transform.forward, inputDirection) != 0 && currentInPutDotProduct <= -0.9)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(180 * playerRotationSpeed * Time.deltaTime, Vector3.up);
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-180 * playerRotationSpeed * Time.deltaTime, Vector3.up);
            }

        }
        #endregion

        if (isPlayerGrounded)
        {
            if (inputDirection != Vector3.zero)
            {
                animator.SetBool("StartRunning", true);
                if (startSprint)
                {
                    MovementForce = MaxPlayerRunSpeed;
                    animator.SetFloat("Movement", 1f);
                }
                else
                {
                    MovementForce = MaxPlayerWalkSpeed;
                    animator.SetFloat("Movement", 0f);
                }


                //rb.velocity = groundPlayerCross;

                groundPlayerCross = Vector3.Cross(gameObject.transform.right, SphereCastInfo.normal) * MovementForce * Time.deltaTime;
                transform.Translate(groundPlayerCross, Space.World);

                //rb.AddForce( Vector3.Cross(gameObject.transform.right, SphereCastInfo.normal) * MovementForce * Time.deltaTime);
                //rb.AddForce(groundPlayerCross);
                Debug.DrawRay(gameObject.transform.position, Vector3.Cross(gameObject.transform.right, SphereCastInfo.normal) * 4, Color.green);
            }
            else
            {
               // rb.velocity = new Vector3(0,rb.velocity.y,0);
                animator.SetBool("StartRunning", false);
            }

            if (jump == true)
            {
                animator.SetTrigger("Jump");
                Debug.Log(jump);
                rb.AddForce(gameObject.transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
               


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

        if (cameraRotation.x < 290 && cameraRotation.x > 55) //cameraRotation.x < 330 && cameraRotation.x > 55
        {
            cameraRotation.x = 290;
        }

        // end clamp
       // Debug.Log(cameraRotation.x);
        CameraTarget.transform.localEulerAngles = cameraRotation;   // reassign the rotation


    }

    // corect the player if there is anyrotation when grounded, make player always upright
    public void PlayerAlwaysUpright()
    {
       
        
        Physics.SphereCast(playerMidPoint.transform.position, SphereCastRadius, -gameObject.transform.up,out SphereCastInfo, SphereCastDistance, sphereCastDetectable, QueryTriggerInteraction.UseGlobal );
        Debug.DrawRay(playerMidPoint.transform.position, -gameObject.transform.up * SphereCastDistance, Color.magenta);

        

         if(gravityScript.justFlippedGravity)
        {

            // recover if there is rotation in players X axis
            if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) >= 0.1)
            {
                if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) < -0.01) //-0.01
                {
                    transform.Rotate(new Vector3(-PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
                  //  moveForceDirection = Quaternion.AngleAxis(30 * Time.deltaTime, Vector3.right) * moveForceDirection;
                }
                else if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) > 0.01) //0.01
                {
                    transform.Rotate(new Vector3(PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
                }
                if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) < 0.6) //0.1
                {
                    gravityScript.justFlippedGravity = false;
                }
            }


            // recover if there is rotation in players Z axis
            if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) >= 0.1) //0.1
            {
                if (Vector3.Dot(gameObject.transform.right, SphereCastInfo.normal) < -0.01) //-0.01
                {
                    transform.Rotate(new Vector3(0, 0, PlayerRecoverySpeed * Time.deltaTime), Space.Self);
                }
                else if (Vector3.Dot(gameObject.transform.right, SphereCastInfo.normal) > 0.01) //0.01
                {
                    transform.Rotate(new Vector3(0, 0, -PlayerRecoverySpeed * Time.deltaTime), Space.Self);
                }

                if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) < 0.6) //0.1
                {
                    gravityScript.justFlippedGravity = false;
                }
            }



        }
        

    }

    public void playerFalling(Vector2 direction)
    {
        RaycastHit hitinfo;
        Debug.DrawRay(playerMidPoint.transform.position, -gameObject.transform.up * fallCheckDistance, Color.white);
        if (Physics.SphereCast(playerMidPoint.transform.position, 0.2f,-gameObject.transform.up, out hitinfo, fallCheckDistance, Walkable))
        // if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hitinfo, fallCheckDistance, Walkable))
        {
            animator.SetBool("falling", false);
        }
        else
        {
            animator.SetBool("falling", true);
        }

        if (!isPlayerGrounded)
        {
            inputDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward);
            //rb.AddForce(inputDirection * airSpeed * Time.deltaTime);
            transform.Translate(inputDirection * airSpeed * Time.deltaTime, Space.World);
            Debug.DrawRay(transform.position, airInputDirection * 5, Color.red); // input direction

            playerRotatingDirection = Mathf.Sign(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right));

            if (Vector3.Angle(gameObject.transform.forward, CameraTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right)) > 0.1)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(RotRecoveryCheckPos.transform.position + -gameObject.transform.up * SphereCastDistance, SphereCastRadius);
        Gizmos.DrawWireSphere(gameObject.transform.position , 0.2f);
    }

}

