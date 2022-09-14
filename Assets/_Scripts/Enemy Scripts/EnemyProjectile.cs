using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody rb;
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
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if(other.gameObject.CompareTag("Bullet Collection"))
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }


}
