using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class RangedEnemy : EnemyBase
{
    bool isGrounded;
    bool canChangeStates;
    public bool canGroundCheck = true;
    GameObject player;
    Transform playerTransform;
    //private NavMeshAgent navAgent;
    private float distanceFromPlayer;
    bool isAttacking;
    private float attackTimer;
    Rigidbody rb;

    public enum State
    {
        Disabled,
        Idle,
        Chasing,
        Fleeing,
        Attacking,
        Stunned,
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

    [Header("AI Debugging")]
    public TMP_Text stateText;

    [Header("Layermask")]
    public LayerMask whatIsPlayer;
    public LayerMask whatIsNotGround;

    new void Awake()
    {
        base.Awake();
        if (TryGetComponent(out NavMeshAgent nav))
        {
            navAgent = nav;
        }
        if (TryGetComponent(out Rigidbody rb))
        {
            this.rb = rb;
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

    //public void ToggleUpdatePosition(bool toggle)
    //{
    //    navAgent.updatePosition = toggle;
    //}

    public void DisabledGroundCheck()
    {
        canGroundCheck = false;
        isGrounded = false;
        navAgent.enabled = false;
        //currentState = State.Stunned;
        //navAgent.updatePosition = false;
        Invoke("ReenabledGC", 3);
    }

    public void ReenabledGC()
    {
        canGroundCheck = true;
        navAgent.enabled = true; //SO TURNING OFF THE GODDAMN FUCKING NAVAGENT JUST WORKS.... FIGURE THIS SHIT OUT TOMORROW.
        //navAgent.updatePosition = true;

    }

    private void Update()
    {
        if (canGroundCheck)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f, ~whatIsNotGround);
        }

        //isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f, ~whatIsNotGround);

        if (isGrounded)
        {
            rb.drag = 10;
            canChangeStates = true;
            navAgent.updatePosition = true;
        }
        else
        {
            rb.drag = 0;
            canChangeStates = false;
            //currentState = State.Stunned;
            //navAgent.nextPosition = transform.position;
            navAgent.updatePosition = false;
        }


        if (stateText != null)
        {
            stateText.text = currentState.ToString();
        }

        distanceFromPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (isDisabled)
        {
            currentState = State.Disabled;
        }
        else if(canChangeStates)
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
                case State.Stunned:
                    StunnedState();
                    break;

            }
        }
    }

    public void StunnedState()
    {
        //navAgent.SetDestination(transform.position);
        navAgent.nextPosition = transform.position;
        //Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit, 100, ~whatIsNotGround);
        //navAgent.nextPosition = hit.point;
    }

    public void ChasingState()
    {
        navAgent.speed = pursueSpeed;
        navAgent.SetDestination(playerTransform.position);
        if(distanceFromPlayer < pursueRange)
        {
            currentState = State.Attacking;
        }
    }

    public void FleeingState()
    {
        navAgent.speed = fleeSpeed;

        Vector3 dirToPlayer = transform.position - playerTransform.position;

        Vector3 newPos = transform.position + dirToPlayer * 3f;

        navAgent.SetDestination(newPos);

        if (distanceFromPlayer > fleeRange + 5)
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
