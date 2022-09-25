using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : EnemyBase
{
    GameObject player;
    Transform playerTransform;
    //private NavMeshAgent navAgent;
    private float distanceFromPlayer;
    bool isAttacking;

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

    [Header("Layermask")]
    public LayerMask whatIsPlayer;

    new void Awake()
    {
        base.Awake();
        if (TryGetComponent(out NavMeshAgent nav))
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
                case State.Attacking:
                    AttackingState();
                    break;
            }
        }
    }

    private void ChasingState()
    {
        navAgent.SetDestination(playerTransform.position);

        if(distanceFromPlayer < meleeRange)
        {
            Debug.Log("Within Melee Range");
            currentState = State.Attacking;
        }
    }

    private void AttackingState()
    {
        if (distanceFromPlayer > meleeRange && !isAttacking)
        {
            currentState = State.Chasing;
        }

        if (!isAttacking)
        {
            StartCoroutine(Lunge());
        }
    }

    private void MeleeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(meleeAttackPosition.position, meleeAttackRadius, whatIsPlayer);
        foreach(Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                if (c.GetComponent<PlayerStats>())
                {
                    c.GetComponent<PlayerStats>().TakeDamage(meleeDamage);
                }
            }
            Debug.Log(c.name);

        }
    }

    public override void ToggleAI()
    {
        isDisabled = !isDisabled;
        if (!isDisabled)
        {
            currentState = State.Chasing;
        }

    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(meleeAttackPosition.position, meleeAttackRadius);
    }

    public IEnumerator Lunge()
    {
        navAgent.ResetPath();
        isAttacking = true;
        Vector3 dirToPlayer = playerTransform.position - transform.position;
        yield return new WaitForSeconds(0.3f);
        //transform.position += dirToPlayer * 150 * Time.deltaTime;
        float elaspedTime = 0;
        Vector3 origPos = transform.position;
        Vector3 targetPos = transform.position + transform.forward * 10;
        //GetComponent<CapsuleCollider>().enabled = false;
        MeleeAttack();
        while (elaspedTime < 0.3f)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elaspedTime / 0.3f));
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(1f);
        GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        currentState = State.Chasing;
        isAttacking = false;

        yield return null;
    }
}
