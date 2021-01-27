using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
  
    
    [SerializeField]
    private Camera TPScam;
    [SerializeField]
    ParticleSystem bullets;
    [SerializeField]
    GameObject cameraFollowTarget;

    //movement
    public Animator animator;
    Rigidbody rb;
    private Vector3 movementDirection;
    [SerializeField]
    float MovementForce = 10000;

    private float playerRotatingDirection;
    [SerializeField]
    private float playerRotatingSpeed = 50;
    private Vector3 inputDirection;
    void Start()
    {
        bullets.enableEmission = false;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void ShootingMovement(Vector2 movementInput, bool startAiming)
    {
        animator.SetBool("StartShooting", startAiming);
        animator.SetFloat("ShootX", movementInput.x);
        animator.SetFloat("ShootY", movementInput.y);

        inputDirection = (movementInput.x * gameObject.transform.right + movementInput.y * gameObject.transform.forward);
        rb.AddForce(inputDirection * MovementForce * Time.deltaTime);
        



        playerRotatingDirection = Mathf.Sign(Vector3.Dot(cameraFollowTarget.transform.forward, gameObject.transform.right));

           if (Vector3.Angle(gameObject.transform.forward, cameraFollowTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(cameraFollowTarget.transform.forward, gameObject.transform.right)) > 0.08)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotatingSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotatingSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
            }


          
        

        
    }
    public void Hitscan(bool StartShootingParticle)
    {
        

        if (StartShootingParticle)
        {
            bullets.enableEmission = true;
            
            bullets.transform.rotation = cameraFollowTarget.transform.rotation;

            Ray HitscanRay = TPScam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
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

   
}
