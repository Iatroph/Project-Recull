using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : EnemyBase
{
    GameObject player;
    Transform playerTransform;
    private NavMeshAgent navAgent;
    private float distanceFromPlayer;
    public enum State
    {
        Disabled,
        Idle,
        Chasing,
        Attacking,
    }

    [HideInInspector]
    public State currentState;

    [Header("Attack Parameters")]
    public float meleeDamage;
    public float meleeRange;
    public float meleeAttackRadius;
    public Transform meleeAttackPosition;

    [Header("NavAgent Parameters")]
    public float chaseSpeed;

    [Header("Other")]
    public bool isDisabled;

    new void Awake()
    {
        base.Awake();
        if(TryGetComponent(out NavMeshAgent nav))
        {
            navAgent = nav;
        }

        navAgent.speed = chaseSpeed;

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

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

        switch (currentState)
        {
            case State.Disabled:
                break;
            case State.Idle:
                break;
            case State.Chasing:
                ChasingState();
                break;
            case State.Attacking:
                break;
        }
    }

    private void ChasingState()
    {
        navAgent.SetDestination(playerTransform.position);

        if(distanceFromPlayer < meleeRange)
        {
            Debug.Log("Within Melee Range");
        }
    }

    private void AttackingState()
    {

    }
}
