using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    GameObject player;
    Transform playerTransform;
    private NavMeshAgent navAgent;
    private float distanceFromPlayer;
    bool isAttacking;
    private float attackTimer;

    public enum State
    {
        Disabled,
        Idle,
        Chasing,
        Fleeing,
        Attacking,
    }

    [HideInInspector]
    public State currentState;

    [Header("Object References")]
    public GameObject projectile;

    [Header("Attack Parameters")]
    public float projectileDamage;
    public Transform projectileSpawn;
    public float attackInterval;

    [Header("Range Parameters")]
    public float pursueRange;
    public float fleeRange; 

    [Header("NavAgent Parameters")]
    public float pursueSpeed;
    public float fleeSpeed;

    [Header("Layermask")]
    public LayerMask whatIsPlayer;

    new void Awake()
    {
        base.Awake();
        if (TryGetComponent(out NavMeshAgent nav))
        {
            navAgent = nav;
        }

        navAgent.speed = pursueSpeed;

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        attackTimer = attackInterval;

    }

    private void Start()
    {
        if (isDisabled)
        {
            currentState = State.Disabled;
        }
        else
        {
            currentState = State.Chasing;
        }
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (isDisabled)
        {
            currentState = State.Disabled;
        }
        else
        {
            switch (currentState)
            {
                case State.Disabled:
                    break;
                case State.Idle:
                    break;
                case State.Chasing:
                    ChasingState();
                    break;
                case State.Fleeing:
                    FleeingState();
                    break;
                case State.Attacking:
                    AttackingState();
                    break;
            }
        }
    }

    public void ChasingState()
    {
        navAgent.SetDestination(playerTransform.position);
        if(distanceFromPlayer < pursueRange)
        {
            currentState = State.Attacking;
        }
    }

    public void FleeingState()
    {
        //Vector3 relativePos = playerTransform.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(-relativePos, Vector3.up);
        //transform.rotation = rotation;

        Vector3 dirToPlayer = transform.position - playerTransform.position;

        Vector3 newPos = transform.position + dirToPlayer * 1.5f;

        navAgent.SetDestination(newPos);

        if (distanceFromPlayer > fleeRange + 2)
        {
            currentState = State.Chasing;
        }
    }

    public void AttackingState()
    {
        navAgent.ResetPath();
        RotateTowardPlayer();
        if (!isAttacking)
        {
            if (distanceFromPlayer > pursueRange)
            {
                currentState = State.Chasing;
            }

            if(distanceFromPlayer < fleeRange)
            {
                currentState = State.Fleeing;
            }
        }

        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            attackTimer = attackInterval;
            Vector3 dirToPlayer = playerTransform.position - projectileSpawn.position;

            GameObject proj = Instantiate(projectile, projectileSpawn.position, Quaternion.LookRotation(dirToPlayer));
            proj.GetComponent<EnemyProjectile>().damage = projectileDamage;


        }

    }

    public void RotateTowardPlayer()
    {
        Vector3 relativePos = playerTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    public override void ToggleAI()
    {
        isDisabled = !isDisabled;
        if (!isDisabled)
        {
            currentState = State.Chasing;
        }
    }
}
