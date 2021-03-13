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
        m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<BoxCollider>();
        dropableObjects = GetComponentsInChildren<Rigidbody>();
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin)
    {
        Debug.Log(damage);
        health -= damage;
        UpdateHealth();
        Vector3 forceDirection = location - direction;
        m_rb.AddForceAtPosition(direction.normalized * damage, location,ForceMode.Impulse);
    }

    void UpdateHealth()
    {
        if (health <= 0)
        {
            m_rb.isKinematic = false;
            m_collider.enabled = false;
            foreach(Rigidbody rb in dropableObjects)
            {
                rb.isKinematic = false;
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
