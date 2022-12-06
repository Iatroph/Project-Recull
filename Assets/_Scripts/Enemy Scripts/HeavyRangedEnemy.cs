using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HeavyRangedEnemy : EnemyBase
{
    GameObject player;
    Transform raycastTarget;

    private float distanceFromPlayer;
    private Vector3 directionToPlayer;

    bool playerInRange;

    bool canSeePlayer;

    bool isAttacking;

    float attackTimer;

    private float attackInterval;

    [Header("References")]
    public GameObject projectile;

    [Header("Particle Effects")]
    public GameObject deathExplosionParticle;

    [Header("Attack Parameters")]
    public float projectileDamage;
    public Transform projectileSpawnRight;
    public Transform projectileSpawnLeft;
    public float initialAttackInterval;
    public float baseAttackInterval;
    public float attackIntervalModifier;

    [Header("Range Parameters")]
    public float pursueRange;

    [Header("NavAgent Parameters")]
    public float pursueSpeed;

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

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, 50, ~ignore))
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
            if (distanceFromPlayer < pursueRange && canSeePlayer)
            {
                AttackingState();
            }

            if ((distanceFromPlayer > pursueRange && !isAttacking) || !canSeePlayer)
            {
                PursueState();
            }

            if (canSeePlayer)
            {
                RotateTowardPlayer();
            }
        }

    }

    public void AttackingState()
    {
        navAgent.ResetPath();
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            isAttacking = true;
            StartCoroutine(RapidFireAttack());
            attackInterval += Random.Range(-attackIntervalModifier, attackIntervalModifier);
            attackTimer = attackInterval;
            attackInterval = baseAttackInterval;
        }
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

    public IEnumerator RapidFireAttack()
    {
        Vector3 dirToPlayer = raycastTarget.position /*+ new Vector3(0, 0.2f, 0)) */- projectileSpawnLeft.position;
        Vector3 dirToPlayer2 = raycastTarget.position /*+ new Vector3(0, 0.2f, 0))*/ - projectileSpawnRight.position;
        yield return new WaitForSeconds(0.01f);
        GameObject proj = Instantiate(projectile, projectileSpawnLeft.position, Quaternion.LookRotation(dirToPlayer));
        proj.GetComponent<EnemyProjectile>().damage = projectileDamage;
        yield return new WaitForSeconds(0.1f);
        GameObject proj2 = Instantiate(projectile, projectileSpawnRight.position, Quaternion.LookRotation(dirToPlayer2));
        proj2.GetComponent<EnemyProjectile>().damage = projectileDamage;
        yield return new WaitForSeconds(0.1f);
        GameObject proj3 = Instantiate(projectile, projectileSpawnLeft.position, Quaternion.LookRotation(dirToPlayer));
        proj3.GetComponent<EnemyProjectile>().damage = projectileDamage;
        yield return new WaitForSeconds(0.1f);
        GameObject proj4 = Instantiate(projectile, projectileSpawnRight.position, Quaternion.LookRotation(dirToPlayer2));
        proj4.GetComponent<EnemyProjectile>().damage = projectileDamage;
        isAttacking = false;
        yield return null;
    }
}
