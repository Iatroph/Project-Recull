using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicRangedEnemy : EnemyBase
{
    GameObject player;
    Transform raycastTarget;

    private float distanceFromPlayer;
    private Vector3 directionToPlayer;

    bool playerInRange;

    bool canSeePlayer;

    bool isAttacking;

    bool isFleeing;

    float attackTimer;

    private float attackInterval;

    [Header("References")]
    public GameObject projectile;

    [Header("Particle Effects")]
    public GameObject deathExplosionParticle;

    [Header("Attack Parameters")]
    public float projectileDamage;
    public Transform projectileSpawnRight;
    public float initialAttackInterval;
    public float baseAttackInterval;
    public float attackIntervalModifier;

    [Header("Range Parameters")]
    public float pursueRange;
    public float fleeRange;

    [Header("NavAgent Parameters")]
    public float pursueSpeed;
    public float fleeSpeed;

    [Header("Layermasks")]
    public LayerMask ignore;

    new void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        raycastTarget = GameObject.FindGameObjectWithTag("Raycast Target").transform;
        navAgent.speed = pursueSpeed;
        attackInterval = initialAttackInterval;
        attackTimer = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        directionToPlayer = raycastTarget.position - transform.position;

        if(Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, 50, ~ignore))
        {
            if (hit.transform.CompareTag("Player"))
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }

        if (!isDisabled)
        {
            if (distanceFromPlayer < pursueRange && !isFleeing && canSeePlayer)
            {
                AttackingState();
            }

            if (distanceFromPlayer > pursueRange || !canSeePlayer)
            {
                PursueState();
            }

            if ((distanceFromPlayer < fleeRange) && canSeePlayer)
            {
                isFleeing = true;
                FleeingState();
            }
            else
            {
                isFleeing = false;
                //RotateTowardPlayer();
            }

            if(canSeePlayer && !isFleeing)
            {
                RotateTowardPlayer();
            }

        }
    }

    public void AttackingState()
    {
        navAgent.ResetPath();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Vector3 dirToPlayer = (player.transform.position + new Vector3(0, 0.2f, 0)) - projectileSpawnRight.position;
            GameObject proj = Instantiate(projectile, projectileSpawnRight.position, Quaternion.LookRotation(dirToPlayer));
            proj.GetComponent<EnemyProjectile>().damage = projectileDamage;
            attackInterval += Random.Range(-attackIntervalModifier, attackIntervalModifier);
            attackTimer = attackInterval;
            attackInterval = baseAttackInterval;
        }
    }

    public void FleeingState()
    {

        navAgent.speed = fleeSpeed;

        Vector3 dirToPlayer = transform.position - player.transform.position;

        Vector3 newPos = transform.position + dirToPlayer * 5f;

        navAgent.SetDestination(newPos);

    }

    public void PursueState()
    {
        navAgent.SetDestination(player.transform.position);
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
