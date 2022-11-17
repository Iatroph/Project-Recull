using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SawbladeEnemy : EnemyBase
{
    private float tickTimer;
    GameObject player;
    private float distanceFromPlayer;
    private Vector3 directionToPlayer;

    [Header("Particle Effects")]
    public GameObject deathExplosionParticle;

    [Header("Attack Parameters")]
    public float meleeDamage;
    public float tickDamage;
    public float tickTime;

    [Header("NavAgent Parameters")]
    public float pursueSpeed;

    [Header("Layermasks")]
    public LayerMask ignore;

    new void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent.speed = pursueSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        tickTimer = tickTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDisabled)
        {
            navAgent.SetDestination(player.transform.position);
            RotateTowardPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(meleeDamage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                tickTimer = tickTime;
                collision.gameObject.GetComponent<PlayerStats>().TakeDamage(tickDamage);

            }
        }
    }

    public void RotateTowardPlayer()
    {
        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;

    }

    public override void Die()
    {
        Instantiate(deathExplosionParticle, transform.position, Quaternion.identity);
        base.Die();
    }

}
