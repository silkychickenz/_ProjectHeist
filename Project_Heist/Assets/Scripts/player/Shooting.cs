﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Camera TPScam;
    [SerializeField]
    
    void Start()
    {
        
    }

    public void Hitscan()
    {
        Ray HitscanRay = TPScam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit HitscanHitinfo;
        
        if (Physics.Raycast(HitscanRay, out HitscanHitinfo))
        {
            if (HitscanHitinfo.transform.gameObject.tag == "Enemy")
            {
                // HitscanHitinfo.transform.gameObject.GetComponent<healthscript>().DamageFunction();   modify this line and make enemies take damage
                print("I'm looking at " + HitscanHitinfo.transform.name);
            }
            
        }
       
           
       
    }
}
