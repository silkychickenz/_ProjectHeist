﻿using System.Collections;
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

    void Start()
    {
        bullets.enableEmission = false;
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
