using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperbodyLookatTarget : MonoBehaviour
{
    public float DistanceFromCam;
    public Camera TPScam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray HitscanRay = TPScam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit HitscanHitinfo;
        Physics.Raycast(HitscanRay, out HitscanHitinfo);


    }
}
