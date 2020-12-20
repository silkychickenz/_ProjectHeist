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
    Animator animator;
    private Vector3 movementDirection;
    [SerializeField]
    private GameObject CameraTarget;
    private float playerRotatingDirection;
    [SerializeField]
    private float playerRotatingSpeed = 50;

    void Start()
    {
        bullets.enableEmission = false;
        animator = gameObject.GetComponent<Animator>();
    }

    public void ShootingMovement(Vector2 movementInput)
    {
        Debug.DrawRay(transform.position, movementDirection * 5, Color.green); // input direction
        movementDirection = (movementInput.x * CameraTarget.transform.right + movementInput.y * CameraTarget.transform.forward);
        Debug.Log(movementDirection);
        animator.SetBool("startShooting", true);
       // animator.SetFloat("shootingX", movementDirection.x);
       // animator.SetFloat("shootingY", movementDirection.z);

        animator.SetFloat("shootingX", movementInput.x);
        animator.SetFloat("shootingY", movementInput.y);


       
            playerRotatingDirection = Mathf.Sign(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right));

            if (Vector3.Angle(gameObject.transform.forward, CameraTarget.transform.forward) != 0 && Mathf.Abs(Vector3.Dot(CameraTarget.transform.forward, gameObject.transform.right)) > 0.08)
            {
                gameObject.transform.rotation *= Quaternion.AngleAxis(playerRotatingDirection * playerRotatingSpeed * Time.deltaTime, Vector3.up); // rptate the player in desired direction
                CameraTarget.transform.rotation *= Quaternion.AngleAxis(-playerRotatingDirection * playerRotatingSpeed * Time.deltaTime, Vector3.up); // rotate camera in opposite direction of player to compensate player rotation
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
