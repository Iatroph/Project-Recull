using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class ChaseEnemy : EnemyBase
{
    bool isGrounded;
    bool canChangeStates = true;
    public bool canGroundCheck = true;
    GameObject player;
    Transform playerTransform;
    //private NavMeshAgent navAgent;
    private float distanceFromPlayer;
    bool isAttacking;
    Rigidbody rb;

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
    public void DisableGroundCheck()
    {
        canGroundCheck = false;
        isGrounded = false;
        Invoke("ReenabledGC", 1f);
    }

    public void ReenabledGC()
    {
        canGroundCheck = true;
    }

    public override void Knockback(Vector3 dir, float force, float upForce)
    {
        isAttacking = false;
        DisableGroundCheck();
        StopAllCoroutines();
        currentState = State.Chasing;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isGrounded = false;
        rb.AddForce(dir * force, ForceMode.Impulse);
        rb.AddForce(transform.up * upForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (canGroundCheck)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.4f, ~whatIsNotGround);
        }

        if (isGrounded)
        {
            rb.drag = 1;
            navAgent.enabled = true;
        }
        else
        {
            rb.drag = 0;
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), 1 * Time.deltaTime);
            rb.AddForce(Vector3.down * 5, ForceMode.Force);
            navAgent.enabled = false;

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
       else if(canChangeStates && isGrounded)
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

        if(Physics.CheckSphere(meleeAttackPosition.position, meleeAttackRadius, whatIsPlayer))
        {
            if (distanceFromPlayer < meleeRange)
            {
                Debug.Log("Within Melee Range");
                currentState = State.Attacking;
            }
        }
        else if (distanceFromPlayer < meleeRange - 1)
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
        RotateTowardPlayer();
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

    public void RotateTowardPlayer()
    {
        Vector3 relativePos = playerTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
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
        navAgent.enabled = false;
        DisableGroundCheck();
        isAttacking = true;
        Vector3 dirToPlayer = playerTransform.position - transform.position;
        yield return new WaitForSeconds(0.2f);
        //GetComponent<CapsuleCollider>().enabled = false;
        //transform.position += dirToPlayer * 150 * Time.deltaTime;
        //float elaspedTime = 0;
        //Vector3 origPos = transform.position;
        //Vector3 targetPos = transform.position + transform.forward * 10;
        //GetComponent<CapsuleCollider>().enabled = false;
        rb.AddForce(dirToPlayer * 10, ForceMode.VelocityChange);
        MeleeAttack();
        //while (elaspedTime < 0.3f)
        //{
        //    transform.position = Vector3.Lerp(origPos, targetPos, (elaspedTime / 0.3f));
        //    elaspedTime += Time.deltaTime;
        //    yield return null;
        //}
        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.3f);
        //GetComponent<CapsuleCollider>().enabled = true;
        navAgent.enabled = true;
        currentState = State.Chasing;
        isAttacking = false;

        yield return null;
    }
}
