using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : HealthManager
{
    Rigidbody m_rb;
    BoxCollider m_collider;
    public float health = 1;
    public float disapearTime = 3;
    Rigidbody[] dropableObjects;

    // Start is called before the first frame update
    void Start()
    {
        //m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<BoxCollider>();
        dropableObjects = GetComponentsInChildren<Rigidbody>();
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin)
    {
        Debug.Log(damage);
        health -= damage;        
        Vector3 forceDirection = direction.normalized * 2;        
        UpdateHealth(location,forceDirection);
    }

    void UpdateHealth(Vector3 location, Vector3 forceDirection)
    {
        if (health <= 0)
        {
            //m_rb.isKinematic = false;
            m_collider.enabled = false;
            foreach(Rigidbody rb in dropableObjects)
            {
                rb.isKinematic = false;
                rb.AddForceAtPosition(forceDirection, location, ForceMode.Impulse);
            }
            StartCoroutine("DisapearLater");
        }
        
    }

    IEnumerator DisapearLater()
    {
        yield return new WaitForSeconds(disapearTime);
        Destroy(this.gameObject);
    }

}
