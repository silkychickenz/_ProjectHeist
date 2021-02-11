using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


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
    private float camDistMin, camDistMax, transitionSpeed = 0;
    public float t = 0;
    [SerializeField]
    private CinemachineVirtualCamera vCam;

    [SerializeField]
    private float cameraRotationSpeed = 10;     // how fast does the camera rotate?
    [SerializeField]
    private float playerRotatingWithCameraSpeed = 10, maxCameraRotation = 60;     // how fast does the camera rotate?
    
    private Vector3 cameraRotation; // store camera rotation
    

    [Header("player rotation recovery")]
    [SerializeField]
    private float PlayerRecoverySpeed = 10;
    [SerializeField]
    float SphereCastRadius = 2;
    [SerializeField]
    float SphereCastDistance = 2f;    
    public GameObject playerMidPoint;
    [SerializeField]
    private LayerMask sphereCastDetectable;
    Gravity gravityScript;
    RaycastHit SphereCastInfo;
    bool cleanUpX, cleanUpZ = true;

   

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
    Vector3 groundPlayerCross;

    [Header(" PLAYER FALLING")]
    [SerializeField]
    private float fallCheckDistance = 15;
    [SerializeField]
    float airSpeed = 5;
    private Vector3 airInputDirection;

    [Header(" PLAYER COVER")]
    public bool isCoverDetected;
    [SerializeField]
    private float coverDetectionDist = 2;
    [SerializeField]
    private LayerMask detectAsCover;
    private GameObject takeCoverOn;
    RaycastHit coverScaninfo;
    RaycastHit coverScanGroundinfo;
    private Vector3 playerToCover;
    Vector3 playerToCoverCross;
    bool runningToCover = false;
    bool canPeakCoverRight, canPeakCoverLeft;
    RaycastHit rightCoverScaninfo, leftCoverScaninfo;
    Vector3 cameraDefaultPos;
    Vector3 downRayCast;

    [Header("Vaulting")]
    [SerializeField]
    private CapsuleCollider defaultPlayerCollider;
    [SerializeField]
    private BoxCollider playerVaultCollider;
    [SerializeField]
    private float vaultDetectionDist = 0.3f;
    RaycastHit forwardVaultCheckRayInfo;
    RaycastHit backVaultCheckRayInfo;
    RaycastHit forwardVaultCheckRayInfoMid;
    bool animateVault = false,animateHighVault = false, animateJump = false;
    bool jump;
   



    void Start()
    {
        animator = playerAvatar.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        gravityScript = gameObject.GetComponent<Gravity>();
       
        cameraDefaultPos = CameraTarget.transform.localPosition;
        cleanUpX = true;
        cleanUpZ = true;
        playerVaultCollider.enabled = false;
        defaultPlayerCollider.enabled = true;
    }

    // player walk/run
    public void MovementAnimation(Vector2 direction, bool jump, bool startSprint, bool startAiming)
    {
        //restore camera zoom and distance when in cover
        if (t <= camDistMax && !startAiming)
        {
            // camera zoom out
            vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = t;
            t += transitionSpeed * Time.deltaTime;
            //camera sholder offset recovery
            if (canPeakCoverRight)
            {
                CameraTarget.transform.Translate(gameObject.transform.right * Time.deltaTime * -t, Space.World);
            }
            if (canPeakCoverLeft)
            {
                CameraTarget.transform.Translate(gameObject.transform.right * Time.deltaTime * t * 2, Space.World);

            }

        }
        //correct remaining inaccuracy after offset recovery
        if(t > camDistMax && !startAiming && Vector3.Distance(cameraDefaultPos, CameraTarget.transform.localPosition) > 0.1)
        {
            CameraTarget.transform.localPosition = cameraDefaultPos;
        }

        animator.SetLayerWeight(1, 0);
        animator.SetBool("startCrouching", false);
        if (isPlayerGrounded)
        {
   
            animator.SetFloat("moveX", direction.x);
            if (direction == Vector2.zero)
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 0);
                direction = Vector2.zero;
            }
            if (direction != Vector2.zero)
            {
                if (startSprint && direction.x == 0 && direction.y > 0)
                {
                    
                    animator.SetFloat("moveY", direction.y + 1);
                }
                else
                {
                  
                    animator.SetFloat("moveY", direction.y);
                }

            }
        }
        


    }

    public void ShootingMovementAnimation(Vector2 movementInput, bool startAiming, bool startShooting,  bool canShoot)
    {
        if (t >= camDistMin)
        {   
            // zoom in
            vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = t; 
            t -= transitionSpeed * Time.deltaTime;

            //sholder offset
            if (canPeakCoverRight)
            {
                CameraTarget.transform.Translate(gameObject.transform.right * Time.deltaTime * t , Space.World);
            }
            if (canPeakCoverLeft)
            {
                CameraTarget.transform.Translate(-gameObject.transform.right * Time.deltaTime * t*2, Space.World);
            }

        }
        if (canShoot && startShooting)
        {
            animator.SetLayerWeight(3, 1);
            animator.SetTrigger("recoil");

        }
        if(canShoot == false)
        {
            animator.ResetTrigger("recoil");
            
        }
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
        if (t <= camDistMax && !startAiming)
        {
            vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = t;
            t += transitionSpeed * Time.deltaTime;

        }
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

    

    //rotate the thirdperson camera
   

    public void Movement(Vector2 direction, bool jump, bool startSprint, bool startCrouching, bool startAiming, bool crouchBoost, bool takeCover)
    {
       // groundcheck
        isPlayerGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, Walkable);
        if (takeCover)
        {
            MovementForce = MaxPlayerCrouchSpeed;
            direction.y = 0;
            if (canPeakCoverRight && direction.x > 0) //trying to go right
            {
                direction.x = 0;
            }
            if (canPeakCoverLeft && direction.x < 0) //trying to go left
            {
                direction.x = 0;
            }
        }
        inputDirection = (direction.y * gameObject.transform.right + direction.x * -gameObject.transform.forward);
        playerAvatar.transform.localPosition = Vector3.zero;

        // set player speed
        if (isPlayerGrounded)
        {
            // if (crouchBoost)
            // {
            //rb.AddForce(transform.forward * runToCroushSlideForce, ForceMode.Impulse);
            //}
            
            
            if (startSprint && !startCrouching && !startAiming && !takeCover)
            {
                if (direction.x == 0 && direction.y > 0 )
                {
                   
                        MovementForce = MaxPlayerRunSpeed;
                    
                    
                }
                else
                {
                    MovementForce = MaxPlayerWalkSpeed;
                }
               
            }
            if(startCrouching)
            {
                
                MovementForce = MaxPlayerCrouchSpeed;
            }
            
            if(!startSprint && !startCrouching || startAiming )
            {
                if (!takeCover)
                {
                    MovementForce = MaxPlayerWalkSpeed;
                }
                
                
            }
            if (startAiming)
            {
              //  MovementForce = MaxPlayerWalkSpeed;
            }

            groundPlayerCross = Vector3.Cross(inputDirection, SphereCastInfo.normal) * MovementForce * Time.deltaTime;
            transform.Translate(groundPlayerCross, Space.World);

            


        }
       

        //Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 10, Color.red);
        Debug.DrawRay(gameObject.transform.position, coverScaninfo.point - gameObject.transform.position, Color.black);

        // taking cover
        if (takeCover && isCoverDetected)
        {
            //right
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * 0.25f, Color.red);
            Debug.DrawRay(gameObject.transform.position + (gameObject.transform.right * 0.25f), gameObject.transform.forward * 1, Color.green);
            Physics.Raycast(gameObject.transform.position + (gameObject.transform.right * 0.25f), gameObject.transform.forward, out rightCoverScaninfo, 1, detectAsCover); //right

            //left
            Debug.DrawRay(gameObject.transform.position, -gameObject.transform.right * 0.25f, Color.red);
            Debug.DrawRay(gameObject.transform.position + (-gameObject.transform.right * 0.25f), gameObject.transform.forward * 1, Color.green);
            Physics.Raycast(gameObject.transform.position + (-gameObject.transform.right * 0.25f), gameObject.transform.forward, out leftCoverScaninfo, 1, detectAsCover);  //left
            if (rightCoverScaninfo.transform != null)
            {
                canPeakCoverRight = false;
               
            }
            else if (rightCoverScaninfo.transform == null)
            {
                canPeakCoverRight = true;
               
            }
             if (leftCoverScaninfo.transform != null)
            {
                canPeakCoverLeft = false;
               
            }
            else if (leftCoverScaninfo.transform == null)
            {
                canPeakCoverLeft = true;
                
            }

            playerRotatingDirection = Mathf.Sign(Vector3.Dot(gameObject.transform.right, coverScaninfo.normal)); // is players back turned towards the wall

            //has player has reached cover
            if (Vector3.Distance(gameObject.transform.position, coverScanGroundinfo.point) <= 0.1  )
            {
                runningToCover = false;
            }

            // run towards cover
            if (Vector3.Distance(gameObject.transform.position, coverScanGroundinfo.point) > 0.3 && runningToCover)
            {
               // Debug.Log("stuck");
                transform.Translate((coverScanGroundinfo.point - gameObject.transform.position).normalized * MaxPlayerRunSpeed * Time.deltaTime, Space.World); ;
            }
            else
            {
                //Debug.Log(Vector3.Dot(gameObject.transform.right, coverScaninfo.normal));
                // rotate and take cover
                if (Mathf.Abs(Vector3.Dot(gameObject.transform.right, coverScaninfo.normal)) > 0.05)
                {
                    runningToCover = false;
                    //Debug.Log(Vector3.Dot(gameObject.transform.right, coverCastInfoRight.normal));
                    transform.Rotate(new Vector3(0, -playerRotatingDirection * 200 * Time.deltaTime, 0), Space.Self);

                }
            }
            

            
           
        }
        else// not taking cover
        {
            playerRotatingDirection = Mathf.Sign(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right));

            isCoverDetected = false;
            if (Vector3.Angle(gameObject.transform.forward, CameraTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right)) > 0.08) //0.08
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotationSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
            }
        }
        
        
    }

    public void JumpAndVault( bool jumpInput, Vector2 direction, bool startSprint)
    {
        if (jumpInput == true && isPlayerGrounded)
        {
            jump = true;
        }

        if (jump == true )
        {
            // regular vault
            if (forwardVaultCheckRayInfo.distance <= vaultDetectionDist && forwardVaultCheckRayInfo.distance >= 0.1 && forwardVaultCheckRayInfo.transform != null)
            {
                // only vault if upper ray has not detected anything
                if (forwardVaultCheckRayInfoMid.transform == null)
                {
                    if (forwardVaultCheckRayInfo.distance <= 1.3f && !startSprint)
                    {
                        animateVault = true;
                        defaultPlayerCollider.enabled = false;
                        playerVaultCollider.enabled = true;
                        jump = false;
                    }
                    else
                    {
                        animateVault = true;
                        defaultPlayerCollider.enabled = false;
                        playerVaultCollider.enabled = true;
                        jump = false;
                    }

                }

            }
           // long jump
           else if (direction.y == 1 && startSprint)
           {
                 rb.AddForce(gameObject.transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
                 rb.velocity += gameObject.transform.forward * (jumpForce + 100) * Time.deltaTime;
                 animateJump = true;
                 jump = false;

            }
            //regular jump
            else
            {
                 rb.AddForce(gameObject.transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
                 rb.velocity += (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward) * jumpForce / 2 * Time.deltaTime;
                 animateJump = true;
                 jump = false;
            }
            // high vault
            if (forwardVaultCheckRayInfoMid.distance <= vaultDetectionDist && forwardVaultCheckRayInfoMid.distance >= 0.05 && forwardVaultCheckRayInfoMid.transform != null)
            {
                if (forwardVaultCheckRayInfoMid.distance <= 1.3f)
                {
                    animateHighVault = true;
                    defaultPlayerCollider.enabled = true;
                    playerVaultCollider.enabled = false;
                    rb.AddForce(gameObject.transform.up * 1500 * Time.deltaTime, ForceMode.Impulse);
                    jump = false;
                }
                

            }


        }

        Debug.DrawRay(gameObject.transform.position , gameObject.transform.forward * vaultDetectionDist, Color.red);
        #region small vault raycast
        // forward
       
        Debug.DrawRay(gameObject.transform.position + (gameObject.transform.up * 0.9f), gameObject.transform.forward * (vaultDetectionDist), Color.blue);
        Physics.Raycast(gameObject.transform.position + (gameObject.transform.up * 0.9f), gameObject.transform.forward, out forwardVaultCheckRayInfo, vaultDetectionDist, detectAsCover);

        // forward
       
        Debug.DrawRay(gameObject.transform.position + (gameObject.transform.up * 0.9f), -gameObject.transform.forward * vaultDetectionDist, Color.blue);
        Physics.Raycast(gameObject.transform.position + (gameObject.transform.up * 0.9f), -gameObject.transform.forward, out backVaultCheckRayInfo, vaultDetectionDist, detectAsCover);


        #endregion
        #region mid vault raycast
       
        Debug.DrawRay(gameObject.transform.position + (gameObject.transform.up * 1.6f), gameObject.transform.forward * (vaultDetectionDist), Color.blue);
        Physics.Raycast(gameObject.transform.position + (gameObject.transform.up * 1.6f), gameObject.transform.forward, out forwardVaultCheckRayInfoMid, vaultDetectionDist, detectAsCover);

        // forward
       
        Debug.DrawRay(gameObject.transform.position + (gameObject.transform.up * 1.6f), -gameObject.transform.forward * vaultDetectionDist, Color.blue);
        Debug.DrawRay((gameObject.transform.position + (-gameObject.transform.forward * 0.2f)) + (gameObject.transform.up * 0.3f), -gameObject.transform.up * 0.5f, Color.blue);
        #endregion

       
        //check if player has cleard vault object and restore default collider
        if (backVaultCheckRayInfo.distance > 0.1 && backVaultCheckRayInfo.transform != null)
        {
          
            defaultPlayerCollider.enabled = true;
            playerVaultCollider.enabled = false;
        }
 

    }

    public void JumpAndVaultAnimation(bool jump)
    {
       
        if (animateVault)
        {
            animator.SetTrigger("vaultTrig");
            animateVault = false;
        }
       
        if (animateHighVault)
        {

            animator.SetTrigger("HighVaultTrig");
            animateHighVault = false;
        }
        if (animateJump)
        {
            animator.SetTrigger("Jump");
            animateJump = false;
        }


    }
    public void RotateCamera(Vector2 lookDirection)
    {

        CameraTarget.transform.rotation *= Quaternion.AngleAxis(lookDirection.x * cameraRotationSpeed * Time.deltaTime, Vector3.up);
        CameraTarget.transform.rotation *= Quaternion.AngleAxis(lookDirection.y * cameraRotationSpeed * Time.deltaTime, Vector3.right); // up dpwn



        if (Vector3.Angle(CameraTarget.transform.forward, gameObject.transform.forward) < maxCameraRotation)
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

    // corect the player if there is anyrotation when grounded, make player always upright
    public void PlayerAlwaysUpright()
    {

       
        Physics.SphereCast(playerMidPoint.transform.position, SphereCastRadius, -gameObject.transform.up,out SphereCastInfo, SphereCastDistance, sphereCastDetectable, QueryTriggerInteraction.UseGlobal );
        Debug.DrawRay(playerMidPoint.transform.position, -gameObject.transform.up * SphereCastDistance, Color.magenta);

        if (SphereCastInfo.transform != null && gravityScript.justFlippedGravity)
        {

            if (SphereCastInfo.transform.tag != "slope")
            {

                // recover if there is rotation in players X axis
                if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) >= 0.01) // 0.1
                {


                    if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) < -0.05  ) //-0.01
                    {
                        transform.Rotate(new Vector3(-PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
                        

                    }
                    else if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) > 0.05 ) //0.01
                    {
                        transform.Rotate(new Vector3(PlayerRecoverySpeed * Time.deltaTime, 0, 0), Space.Self);
                       
                    }
                    // clean up
                    else if (Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) <= 0.07 && Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) >= -0.07 && cleanUpX) //0.01
                    {
                       
                        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
                        cleanUpX = false;

                       // Debug.Log("DOT = " + Vector3.Dot(gameObject.transform.forward, SphereCastInfo.normal) + "   " + transform.localRotation.eulerAngles + "  ANGLE " + Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal));
                    }

                }
                else
                {
                    cleanUpX = true;
                   
                }


                // recover if there is rotation in players Z axis
                if (Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) >= 0.01) //0.1
                {
                    if (Vector3.Dot(gameObject.transform.right, SphereCastInfo.normal) < -0.05) //-0.01
                    {
                        transform.Rotate(new Vector3(0, 0, PlayerRecoverySpeed * Time.deltaTime), Space.Self);
                       
                    }
                    else if (Vector3.Dot(gameObject.transform.right, SphereCastInfo.normal) > 0.05) //0.01
                    {
                        transform.Rotate(new Vector3(0, 0, -PlayerRecoverySpeed * Time.deltaTime), Space.Self);
                       
                       

                    }    
                    else if (Vector3.Dot(gameObject.transform.right, SphereCastInfo.normal) <= 0.07 && Vector3.Dot(gameObject.transform.right, SphereCastInfo.normal) >= -0.07 && cleanUpZ) //0.01
                    {
                        
                        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, -90);
                        cleanUpZ = false;

                    }

                }
                else
                {
                    cleanUpZ = true;
                   
                }

                if (!cleanUpZ && !cleanUpX && Vector3.Angle(gameObject.transform.up, SphereCastInfo.normal) < 3)
                {
                    gravityScript.justFlippedGravity = false;
                    Debug.Log(gameObject.transform.localRotation.eulerAngles);
                }

            }
        }
       

    }

    public void playerFalling(Vector2 direction)
    {
        RaycastHit hitinfo;
        Debug.DrawRay(playerMidPoint.transform.position, -gameObject.transform.up * fallCheckDistance, Color.white);
        Physics.SphereCast(playerMidPoint.transform.position, 0.2f, -gameObject.transform.up, out hitinfo, Mathf.Infinity, Walkable);

        if(hitinfo.distance >= fallCheckDistance)
        {
            animator.SetBool("falling", true);
            inputDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward);
            //rb.AddForce(inputDirection * airSpeed * Time.deltaTime);
            transform.Translate(inputDirection * airSpeed * Time.deltaTime, Space.World);
            Debug.DrawRay(transform.position, airInputDirection * 5, Color.red); // input direction

        }
        if (hitinfo.distance < fallCheckDistance && hitinfo.distance > 0.1f)
        {
            
            inputDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward);
            animator.SetBool("falling", false);
            transform.Translate(inputDirection * (airSpeed/4) * Time.deltaTime, Space.World);
           

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

    public void CoverDetection()
    {

        
        Ray coverScanRay = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, coverDetectionDist));
        if (Physics.Raycast(coverScanRay, out coverScaninfo))
        {
            if (coverScaninfo.distance <= coverDetectionDist)
            {
                downRayCast = coverScanRay.GetPoint(coverScaninfo.distance - 0.2f);
                if (Physics.Raycast(coverScanRay.GetPoint(coverScaninfo.distance - 0.2f), -gameObject.transform.up, out coverScanGroundinfo, 2, detectAsCover))
                {
                    isCoverDetected = true;
                    runningToCover = true;
                    takeCoverOn = coverScaninfo.transform.gameObject;

                   // Debug.Log("dist " + coverScaninfo.distance);
                }
            }
            
        }
        else
        {
            isCoverDetected = false;
        }
    }

    public void CoverAnimations(bool takeCover)
    {
        animator.SetBool("canPeakCoverRight", canPeakCoverRight);
        if (canPeakCoverRight)
        {
            animator.SetFloat("coverIdlePos", 1);
        }
        animator.SetBool("canPeakCoverLeft", canPeakCoverLeft);
        if (canPeakCoverLeft)
        {
            animator.SetFloat("coverIdlePos", 0);
        }
        if (isCoverDetected && takeCover)
        {
            if (Vector3.Distance(gameObject.transform.position, coverScanGroundinfo.point) < 0.5)
            {
                animator.SetBool("takeCover", true);
                

            }
            else
            {
                animator.SetFloat("moveY", 2);
            }

        }
        else
        {
            animator.SetBool("takeCover", false);
        }
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(RotRecoveryCheckPos.transform.position + -gameObject.transform.up * SphereCastDistance, SphereCastRadius);
        //Gizmos.DrawWireSphere(gameObject.transform.position , 0.2f);
        Gizmos.DrawWireSphere(coverScanGroundinfo.point, 0.1f);
        Gizmos.DrawWireSphere(downRayCast,0.1f);
        
    }

}

