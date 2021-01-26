using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    public Vector3 NormalForceDirection;

    // Update is called once per frame
    void Update()
    {
        ChangeNormalForceDirection();
    }

    void ChangeNormalForceDirection()
    {
        transform.LookAt(Vector3.zero,Vector3.up);
    }
}
