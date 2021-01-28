using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("player gravity")]
    [SerializeField]
    private float gravity = 19;
    [SerializeField]
    public bool isPlayerGrounded = true;
    [SerializeField]
    private float groundCheckDist = 0.2f;
    private float currentGravity = 0;
    private Rigidbody rb;
    private Animator animator;

    [Header("Gravity Flipping")]
    [SerializeField]
    private LayerMask Walkable; // what layer can the player walk on
    [SerializeField]
    private float RayCastLength = 100; // how far to cast the ray?
    [SerializeField]
    private Vector3 raycastDirection;
    [SerializeField]
    private float GravityFlipStartForce = 8; // when player uses gracity flip, a jump for is applied that pushed the player off the ground?
    [SerializeField]
    private float flipRotationSpeed = 10; // how fast does the pl;ayer rotate when gravity is flipped
    private float currentRotationTracker = 0;
    [SerializeField]
    private float MinDistanceBeforeFlip = 2; // how high should player jump before a backflip?
    public bool isEnoughDistFromGround = false;
    private bool tempGravityDisabler = false; // disable the gravity temporaroly?
    private float RotByDegrees = 0;
    private bool Rotating = false;
    public bool justFlippedGravity;

    public bool flipForward;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public void GravityFlip(Vector2 direction, bool CanFlipGravity, bool gravityFlipWheel) // direction gets the input and CanFlipGravity gets the cooldown, 
    {
        
        raycastDirection = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.up); // direction of raycast, flip

        RaycastHit hitSurfaceinfo;
        Physics.Raycast(gameObject.transform.position, raycastDirection, out hitSurfaceinfo, RayCastLength, Walkable);

        Debug.DrawRay(gameObject.transform.position, raycastDirection * RayCastLength, Color.red); // ground check ray visualized

        // STAGE #1 :  add force and starting the gravity flip
        if (raycastDirection != Vector3.zero && CanFlipGravity && !isEnoughDistFromGround && gravityFlipWheel) // if there is input and its not on cooldowm
        {
           
           rb.AddForce(GravityFlipStartForce * gameObject.transform.up, ForceMode.Impulse); // boost the player a little off the ground
           
           animator.SetTrigger("Jump");
            Rotating = true;
            
        }
       
        // STAGE #2 : determine flip rotation and its direction depending on input
        if (direction.y == 1)
        {
            RotByDegrees = -180;
        }

        if (direction.y == -1)
        {
            RotByDegrees = -90; // forward rotation
            flipForward = true;
        }

        if (direction.x == 1)
        {
            RotByDegrees = 90;
        }

        if (direction.x == -1)
        {
            RotByDegrees = -90;
        }

        // STAGE #3 : raycast to check if player is minimum distance from ground to begin flipping
        RaycastHit hitinfoDistance;
        Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hitinfoDistance, Mathf.Infinity, Walkable);
        if (hitinfoDistance.distance > MinDistanceBeforeFlip)
        {
            isEnoughDistFromGround = true;
        }
       
        if (Mathf.Abs(currentRotationTracker) >= Mathf.Abs(RotByDegrees) && hitinfoDistance.distance <= MinDistanceBeforeFlip) // if the player is successfully completed a rotation and is withing minimum distance for flip
        {
            isEnoughDistFromGround = false;
        }
        //new
        if (hitinfoDistance.distance <= MinDistanceBeforeFlip && isPlayerGrounded) // if the player is successfully completed a rotation and is withing minimum distance for flip
        {
            isEnoughDistFromGround = false;
           
        }


        // STAGE #4 : Start Rotating
        if (Rotating && isEnoughDistFromGround)
        {
            tempGravityDisabler = true;

            // STAGE #5 : apply rotation to player
            if (Mathf.Abs(RotByDegrees) == 180 && !flipForward) // rotate vertically
            {

                transform.Rotate(new Vector3(0, 0, RotByDegrees * flipRotationSpeed * Time.deltaTime), Space.Self);
            }

            if (Mathf.Abs(RotByDegrees) == 90 && !flipForward) // rotate right
            {
                transform.Rotate(new Vector3(0, 0, RotByDegrees * flipRotationSpeed * Time.deltaTime), Space.Self);
            }

            if (Mathf.Abs(RotByDegrees) == -90 && !flipForward) // rotate left
            {
                transform.Rotate(new Vector3(0, 0, -RotByDegrees * flipRotationSpeed * Time.deltaTime), Space.Self);
            }

            if (Mathf.Abs(RotByDegrees) == 90 && flipForward) // rotate Forward
            {
                Debug.Log(RotByDegrees);
                transform.Rotate(new Vector3(RotByDegrees * flipRotationSpeed * Time.deltaTime, 0, 0), Space.Self);
                
            }

            currentRotationTracker += (RotByDegrees * flipRotationSpeed * Time.deltaTime); // keep track of how much player has rotated

            // STAGE #6 : clean up the rotation 
            if (Mathf.Abs(currentRotationTracker) >= Mathf.Abs(RotByDegrees) - 10 && currentRotationTracker <= Mathf.Abs(RotByDegrees) + 10)
            {
                if (Mathf.Abs(RotByDegrees) == 180)
                {

                    if (Mathf.Abs(gameObject.transform.eulerAngles.z) <= 190 && Mathf.Abs(gameObject.transform.eulerAngles.z) >= 170)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180);
                    }

                    else if (Mathf.Abs(gameObject.transform.eulerAngles.z) <= 10)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
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

                    if (gameObject.transform.eulerAngles.z <= 10)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);


                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 100 && (gameObject.transform.eulerAngles.z) >= 80)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);

                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 190 && (gameObject.transform.eulerAngles.z) >= 170)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180);

                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 280 && (gameObject.transform.eulerAngles.z) >= 260)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 270);

                    }

                    else if ((gameObject.transform.eulerAngles.z) <= 370 && (gameObject.transform.eulerAngles.z) >= 350)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 360);

                    }
                }


                // STAGE #7 : reset
                tempGravityDisabler = false;
                Rotating = false;
                currentRotationTracker = 0;
                RotByDegrees = 0;
                justFlippedGravity = true;
                flipForward = false;

            }

        }



    }

    // apply gravity
    public void ApplyGravity()
    {
        
       // RaycastHit hitinfo;
        //Debug.DrawRay(gameObject.transform.position, -gameObject.transform.up * groundCheckDist, Color.white);
       // if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hitinfo, groundCheckDist, Walkable))
       
        
            isPlayerGrounded = Physics.CheckSphere(gameObject.transform.position, groundCheckDist, Walkable);
       
        

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
        animator.SetBool("isPlayergrounded", isPlayerGrounded);
    }



   
}
