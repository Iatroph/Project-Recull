using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject impact = Instantiate(impactParticle, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            GameObject impact = Instantiate(impactParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }


}
