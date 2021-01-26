using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator m_animator;
    bool doorOpen;
    private void Start()
    {
        doorOpen = false;
        m_animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
       if(col.gameObject.tag == "Player"|| col.gameObject.tag == "Enemy")
        {
            if(Mathf.Abs(col.gameObject.transform.rotation.eulerAngles.x)<1 && Mathf.Abs(col.gameObject.transform.rotation.eulerAngles.x) > 360)
            {
                if (Mathf.Abs(col.gameObject.transform.rotation.eulerAngles.z) < 1 && Mathf.Abs(col.gameObject.transform.rotation.eulerAngles.z) > 360)
                {
                    doorOpen = true;
                    DoorControl("Open");
                }
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
