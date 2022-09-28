using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedProjectile : MonoBehaviour
{
    Rigidbody rb;
    [Header("References")]
    public GameObject impactParticle;
    [Header("Parameters")]
    public float speed;
    [HideInInspector]
    public float damage { get; set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other) //REDO BULLET COLLECTION COLLIDERS
    {
        if (other.gameObject.GetComponent<Hurtbox>())
        {
            GameObject impact = Instantiate(impactParticle, transform.position, Quaternion.identity);
            Hurtbox hb = other.gameObject.GetComponent<Hurtbox>();
            hb.AdjustDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            GameObject impact = Instantiate(impactParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
