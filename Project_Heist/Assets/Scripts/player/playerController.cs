using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    
    private Animator animator;
    private Rigidbody rb;

    [Header("player movement")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float walkToRunTransationSpeed;
    [SerializeField]
    float WalkToRunBlendLimit = 2;
    public float CurrentBlend , CurrentDirectionalBlend;
    float currentInPutDotProduct;
    [SerializeField]
    private float playerRotationSpeed = 15;
    [SerializeField]
    private LayerMask Walkable; // what layer can the player walk on
    //[SerializeField]
    //public bool isPlayerGrounded = true;
    [SerializeField]
    private float groundCheckDist = 0.2f;
    [SerializeField]
    private float runningGroundCheckDist = 5f;

    

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
    private GameObject RotRecoveryCheckPos;
    [SerializeField]
    private LayerMask sphereCastDetectable;
    private GameObject RecoverOnlyOn;

    [Header("player air movement")]
    [SerializeField]
    float airSpeed = 5;
    private Vector3 airInputDirection;
    


    // camera system
    private Vector3 cameraRotation; // store camera rotation


    // player rotation system
    private Vector3 inputDirection; // vector 3 version of the directional input
    private float playerRotatingDirection;

    [Header(" new player movement")]
    [SerializeField]
    public bool isPlayerGrounded = true;
    [SerializeField]
    private float MaxPlayerWalkSpeed = 5, MaxPlayerRunSpeed = 10, jumpForce = 10;
    private float MovementForce = 0;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // player walk/run
    public void newMovement(Vector2 direction, bool jump, bool startSprint)
    {

            
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
                rb.AddForce(gameObject.transform.forward * MovementForce * Time.deltaTime);
            }
            else
            {
                animator.SetBool("StartRunning", false);
            }

            if (jump == true)
            {
                animator.SetTrigger("Jump");
                rb.AddForce(gameObject.transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);

            }
            
            
        }
        


    }

    public void AirMovemet(Vector2 direction)
    {
        if (!isPlayerGrounded)
        {
            //airInputDirection = (direction.x * CameraTarget.transform.right + direction.y * CameraTarget.transform.forward);
            airInputDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward);
           // Debug.Log(airInputDirection);
            airInputDirection.y = 0;
            rb.AddForce(airInputDirection * airSpeed * Time.deltaTime);
            Debug.DrawRay(transform.position, airInputDirection * 5, Color.red); // input direction

            playerRotatingDirection = Mathf.Sign(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right));

            if (Vector3.Angle(gameObject.transform.forward, CameraTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right)) > 0.1 )   
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
       
        RaycastHit SphereCastInfo;
        Physics.SphereCast(RotRecoveryCheckPos.transform.position, SphereCastRadius, -gameObject.transform.up,out SphereCastInfo, SphereCastDistance, sphereCastDetectable, QueryTriggerInteraction.UseGlobal );
        Debug.DrawRay(RotRecoveryCheckPos.transform.position, -gameObject.transform.up * SphereCastDistance, Color.magenta);

        //Debug.Log("NORMAL : " + Vector3.Angle(gameObject.transform.up, hitinfo.normal ));
        //Debug.Log("DOT : " + Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal));
        // Debug.Log((gameObject.transform.eulerAngles.x));
        // Debug.Log(SphereCastInfo.collider.gameObject.name);
        // Debug.Log(Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal));
       // if(SphereCastInfo.transform.gameObject == RecoverOnlyOn)
       // {

            // recover if there is rotation in players X axis
            if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) >= 0.1)
            {
                if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) < -0.01) //-0.01
            {
                    transform.Rotate(new Vector3(-PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
                }
                else if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) > 0.01) //0.01
            {
                    transform.Rotate(new Vector3(PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
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
            }
       // }
        

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(RotRecoveryCheckPos.transform.position + -gameObject.transform.up * SphereCastDistance, SphereCastRadius);
        Gizmos.DrawWireSphere(gameObject.transform.position , 0.2f);
    }

}

