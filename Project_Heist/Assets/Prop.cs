using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : HealthManager
{
    public bool dealDamage;
    public float damageAmount;
    public AudioClip clang;
    private AudioSource m_audio;
    private Rigidbody m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_audio = GetComponent<AudioSource>();
        m_audio.clip = clang;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_rb.isKinematic)
        {
            m_audio.PlayOneShot(m_audio.clip);

            if (dealDamage)
            {
                if (collision.transform.gameObject.tag == "Enemy")
                {
                    collision.collider.SendMessageUpwards("HitCallback", new HealthManager.DamageInfo(collision.GetContact(0).point, transform.forward, damageAmount, collision.collider), SendMessageOptions.DontRequireReceiver);

                    print(gameObject.name + " hits " + collision.transform.name);
                }
            }
        }
    }
    
}
