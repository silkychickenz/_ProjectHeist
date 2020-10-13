using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowScript : MonoBehaviour
{
    [Header("Get references")]
    [SerializeField]
    private GameObject Target;  // follow this gameobject

    [SerializeField]
    private Vector3 offset;    // adjust position around gameobject being followed in global space

    void Start()
    {
        offset = new Vector3(0.18f,1.53f,-0.02f); // default offset
    }

    public void follow() // start following the target
    {
        transform.position = Target.transform.position + offset;
    }
}
