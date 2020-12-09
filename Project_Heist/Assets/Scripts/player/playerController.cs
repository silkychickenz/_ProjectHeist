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
    [SerializeField]
    private bool isPlayerGrounded = true;
    [SerializeField]
    private float groundCheckDist = 0.2f;

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

    // camera system
    private Vector3 cameraRotation; // store camera rotation


    // player rotation system
    private Vector3 inputDirection; // vector 3 version of the directional input
    private float playerRotatingDirection;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        
    }

    // player walk/run
    public void Movement(Vector2 lookDirection, Vector2 direction, bool startSprinting)
    {
        #region raycast
        RaycastHit hitinfo;
        if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hitinfo, groundCheckDist, Walkable))
        {

            isPlayerGrounded = true;

        }

        else
        {
            isPlayerGrounded = false;
        }
        #endregion

        currentInPutDotProduct = Vector3.Dot(gameObject.transform.forward, inputDirection);
        inputDirection = (direction.x * CameraTarget.transform.right + direction.y * CameraTarget.transform.forward);
 
        #region debug
        Debug.DrawRay(transform.position, inputDirection * 5, Color.green); // input direction
        Debug.DrawRay(CameraTarget.transform.position, CameraTarget.transform.forward * 5, Color.yellow);
        Debug.DrawRay(CameraTarget.transform.position, CameraTarget.transform.right * 5, Color.black);
        #endregion

        #region playerortation
        //player roation when there is movement input
        if (direction != Vector2.zero && isPlayerGrounded)
        {
            playerRotatingDirection = Mathf.Sign(Vector3.Dot(inputDirection.normalized, gameObject.transform.right));
            
            if (Vector3.Angle(gameObject.transform.forward, inputDirection) != 0 && Mathf.Abs(Vector3.Dot(inputDirection.normalized, gameObject.transform.right)) > 0.08)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
            }
            if (Vector3.Angle(gameObject.transform.forward, inputDirection) != 0  && !startSprinting && currentInPutDotProduct <= -0.9)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(180 * playerRotationSpeed * Time.deltaTime, Vector3.up); 
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-180 * playerRotationSpeed * Time.deltaTime, Vector3.up); 
            }



        }
        #endregion

        #region walk
        if (currentInPutDotProduct >= 0.95 && isPlayerGrounded) // go forward
        {
            animator.SetBool("startWalking" , true);
            

        }
        
        #endregion
        #region sprinting
        if (startSprinting)
        {
            if (startSprinting && currentInPutDotProduct >= 0.95) // blend from walk to sprint
            {
                if (CurrentBlend <= 1)
                {
                    CurrentBlend += walkToRunTransationSpeed * Time.deltaTime;
                }

            }
          
            if (currentInPutDotProduct <= -0.92) // REVERSE DIRECTION
            {
                
               CurrentDirectionalBlend = 1;

            }
            else // RESET REVERSE DIRECTION
            {
               
                CurrentDirectionalBlend = 0;
            }

        }
        
        #endregion

        #region cleanup
        if (!isPlayerGrounded)
        {
            animator.SetBool("startWalking", false);
            CurrentDirectionalBlend = 0;
            CurrentBlend = 0;
        }
        else if (inputDirection == Vector3.zero && isPlayerGrounded)
        {
            CurrentDirectionalBlend = 0;
            if (CurrentBlend > 0) // blend from sprint to walk
            {
                CurrentBlend -= walkToRunTransationSpeed * Time.deltaTime;
                
            }
            else
            {
               animator.SetBool("startWalking", false);
            }
            
        }

        if (CurrentBlend > 0 && !startSprinting) // blend from sprint to walk
        {
            CurrentBlend -= walkToRunTransationSpeed * Time.deltaTime;
        }

        #endregion
        animator.SetFloat("MovementDirection_Parameter_blend", CurrentDirectionalBlend);
        animator.SetFloat("Movement_Parameter_blend", CurrentBlend);
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
        Gizmos.DrawWireSphere(RotRecoveryCheckPos.transform.position + -gameObject.transform.up * SphereCastDistance, SphereCastRadius);
    }

}

