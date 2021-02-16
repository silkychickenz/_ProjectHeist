using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator m_animator;
    bool doorOpen;
    public LayerMask opensTo;

    private void Start()
    {
        doorOpen = false;
        m_animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
       if(col.gameObject.layer == 10 )
        {
            Debug.Log(Vector3.Dot(col.gameObject.transform.up, this.gameObject.transform.up));
            if (Vector3.Dot(col.gameObject.transform.up, this.gameObject.transform.up) > 0.5)
            {
                doorOpen = true;
                DoorControl("Open");
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (doorOpen)
        {
            doorOpen = false;
            DoorControl("Close");
        }
    }
    void DoorControl(string direction)
    {
        m_animator.SetTrigger(direction);
    }
}
