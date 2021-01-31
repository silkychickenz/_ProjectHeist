using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crouchMovement : MonoBehaviour
{
    [SerializeField]
    private Camera TPScam;
    [SerializeField]
    GameObject cameraFollowTarget;

    //movement
    [SerializeField]
    private GameObject playerAvatar;
    public Animator animator;
    [SerializeField]
    float MovementForce = 2;
    [SerializeField]
    float runToCroushSlideForce = 2;
    Rigidbody rb;


    private float playerRotatingDirection;
    [SerializeField]
    private float playerRotatingSpeed = 50;
    private Vector3 inputDirection;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = playerAvatar.GetComponent<Animator>();
        
    }

    public void CrouchMovement(Vector2 movementInput, bool startCrouching, bool boost )
    {
        if (boost)
        {
            rb.AddForce(transform.forward * runToCroushSlideForce, ForceMode.Impulse);
        }
        
        playerAvatar.transform.localPosition = Vector3.zero;
        animator.SetBool("StartRunning", false);
        animator.SetBool("startCrouching" , startCrouching);
        animator.SetFloat("crouchX", movementInput.x);
        animator.SetFloat("crouchY", movementInput.y);

        inputDirection = (movementInput.x * gameObject.transform.right + movementInput.y * gameObject.transform.forward);

        transform.Translate(inputDirection * MovementForce * Time.deltaTime, Space.World);




        playerRotatingDirection = Mathf.Sign(Vector3.Dot(cameraFollowTarget.transform.forward, gameObject.transform.right));

        if (Vector3.Angle(gameObject.transform.forward, cameraFollowTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(cameraFollowTarget.transform.forward, gameObject.transform.right)) > 0.08)
        {
            gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotatingSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
            cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotatingSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
        }






    }
}
