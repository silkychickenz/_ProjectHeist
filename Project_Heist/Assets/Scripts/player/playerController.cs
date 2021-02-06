using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    
    public Animator animator;
    private Rigidbody rb;
    [SerializeField]
    private GameObject playerAvatar;
    [SerializeField]
    ParticleSystem bullets;

    [Header("Camera")]
    [SerializeField]
    private GameObject CameraTarget;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float cameraRotationSpeed = 10;     // how fast does the camera rotate?
    [SerializeField]
    private float playerRotatingWithCameraSpeed = 10, maxCameraRotation = 60;     // how fast does the camera rotate?
    
    private Vector3 cameraRotation, cameraRotStore; // store camera rotation
    private Quaternion cameraRotTracker = Quaternion.identity;

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
    private float MaxPlayerWalkSpeed = 5, MaxPlayerRunSpeed = 10, MaxPlayerCrouchSpeed = 2, jumpForce = 10, runToCroushSlideForce = 30;
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
    public void MovementAnimation(Vector2 direction, bool jump, bool startSprint)
    {
        animator.SetLayerWeight(1, 0);
        animator.SetBool("startCrouching", false);
        if (isPlayerGrounded)
        {
            if (jump == true)
            {
                animator.SetTrigger("Jump");
                
            }
           
            animator.SetFloat("moveX", direction.x);
            if (direction == Vector2.zero)
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 0);
                direction = Vector2.zero;
            }
            if (direction != Vector2.zero)
            {
                if (startSprint)
                {

                    animator.SetFloat("moveY", direction.y + 1);
                }
                else
                {

                    animator.SetFloat("moveY", direction.y);
                }

            }
        }
        if (!isPlayerGrounded)
        {
            animator.ResetTrigger("Jump");
        }


    }

    public void ShootingMovementAnimation(Vector2 movementInput, bool startAiming)
    {
        animator.SetBool("StartShooting", startAiming);
        animator.SetFloat("ShootX", movementInput.x);
        animator.SetFloat("ShootY", movementInput.y);
        if (!isPlayerGrounded)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }

    }

    public void CrouchMovementAnimation(Vector2 movementInput, bool startCrouching, bool boost, bool startAiming)
    {
        if (boost)
        {
            //  rb.AddForce(transform.forward * runToCroushSlideForce, ForceMode.Impulse);
        }

        
        animator.SetBool("StartRunning", false);
        animator.SetBool("startCrouching", startCrouching);
        animator.SetFloat("crouchX", movementInput.x);
        animator.SetFloat("crouchY", movementInput.y);
        if (startAiming)
        {
            
        }
        else
        {
           
        }

    }

    public void Hitscan(bool StartShootingParticle)
    {


        if (StartShootingParticle)
        {
            bullets.enableEmission = true;

            bullets.transform.rotation = CameraTarget.transform.rotation;

            Ray HitscanRay = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit HitscanHitinfo;

            if (Physics.Raycast(HitscanRay, out HitscanHitinfo))
            {
                if (HitscanHitinfo.transform.gameObject.tag == "Enemy")
                {
                    HitscanHitinfo.collider.SendMessageUpwards("HitCallback", new HealthManager.DamageInfo(HitscanHitinfo.point, transform.forward, 2, HitscanHitinfo.collider), SendMessageOptions.DontRequireReceiver);

                    print("I'm looking at " + HitscanHitinfo.transform.name);
                }

            }
        }
        if (!StartShootingParticle)
        {
            bullets.enableEmission = false;

        }



    }

    //rotate the thirdperson camera
    public void RotateCamera(Vector2 lookDirection)
    {

        CameraTarget.transform.rotation *= Quaternion.AngleAxis(lookDirection.x * cameraRotationSpeed * Time.deltaTime, Vector3.up);
        CameraTarget.transform.rotation *= Quaternion.AngleAxis(lookDirection.y * cameraRotationSpeed * Time.deltaTime, Vector3.right); // up dpwn
      
        Debug.Log(Vector3.Angle(CameraTarget.transform.forward,gameObject.transform.forward));
        
        if (Vector3.Angle(CameraTarget.transform.forward, gameObject.transform.forward) < maxCameraRotation )
        {
            
            cameraRotation = CameraTarget.transform.localEulerAngles;
        }
        if (Vector3.Angle(CameraTarget.transform.forward, gameObject.transform.forward) >= maxCameraRotation)
        {

            cameraRotation.y = CameraTarget.transform.localEulerAngles.y;

        }

        cameraRotation.z = 0; // eleminate the error in z rotation
        CameraTarget.transform.localEulerAngles = cameraRotation;   // reassign the rotation


    }

    public void Movement(Vector2 direction, bool jump, bool startSprint, bool startCrouching, bool startAiming, bool crouchBoost)
    {
        Debug.DrawRay(gameObject.transform.position, Vector3.Cross(inputDirection, SphereCastInfo.normal) * 4, Color.red);
        isPlayerGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, Walkable);
        inputDirection = (direction.y * gameObject.transform.right + direction.x * -gameObject.transform.forward);
        playerAvatar.transform.localPosition = Vector3.zero;
        if (isPlayerGrounded)
        {
            // if (crouchBoost)
            // {
            //rb.AddForce(transform.forward * runToCroushSlideForce, ForceMode.Impulse);
            //}
            
            Debug.Log(startAiming);
            if (startSprint && !startCrouching && !startAiming )
            {
                
                MovementForce = MaxPlayerRunSpeed;
            }
            if(startCrouching)
            {
                
                MovementForce = MaxPlayerCrouchSpeed;
            }
            
            if(!startSprint && !startCrouching)
            {
                MovementForce = MaxPlayerWalkSpeed;
                
            }

            groundPlayerCross = Vector3.Cross(inputDirection, SphereCastInfo.normal) * MovementForce * Time.deltaTime;
            transform.Translate(groundPlayerCross, Space.World);

            if (jump == true)
            {
                
                rb.AddForce(gameObject.transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);

            }


        }
        
        playerRotatingDirection = Mathf.Sign(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right));

        if (Vector3.Angle(gameObject.transform.forward, CameraTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right)) > 0.08)
        {
            gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
            CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
        }
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

