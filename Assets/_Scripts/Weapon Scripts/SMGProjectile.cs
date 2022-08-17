using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGProjectile : ProjectileBase
{
    private float tickTimer;
    private Collider collidr;
    private TrailRenderer tr;
    private GameObject markedEnemy;

    [Header("Recall Parameters")]
    public float recallDamage;
    public float sawRadius;
    public float tickTime;
    public float trackSpeed;
    public float enemyCheckRange;

    [Header("Other")]
    public Color recallTrailColor;
    public LayerMask whatIsHurtBox;


    new void Awake()
    {
        base.Awake();
        tickTimer = tickTime;
        collidr = GetComponent<Collider>();
        tr = GetComponent<TrailRenderer>();
    }

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
        if (isReturning && markedEnemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, markedEnemy.transform.position, trackSpeed * Time.deltaTime);
        }
    }

    public void CheckForEnemy()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, enemyCheckRange, whatIsHurtBox);

        float dist;
        float storedDist = enemyCheckRange;

        foreach (Collider c in hitEnemies)
        {
            dist = Vector3.Distance(transform.position, c.transform.position);
            if (dist < storedDist)
            {
                storedDist = dist;
                markedEnemy = c.gameObject;
            }

        }

    }

    public override void ActivateRecallAbility()
    {
        CheckForEnemy();
        ReturnToPlayer();
        collidr.GetComponent<CapsuleCollider>().radius = sawRadius;
        tr.startColor = recallTrailColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && isReturning)
        {
            collision.transform.GetComponent<IDamageable>().TakeDamage(recallDamage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && isReturning)
        {
            tickTimer -= Time.deltaTime;
            if(tickTimer <= 0)
            {
                tickTimer = tickTime;
                collision.transform.GetComponent<IDamageable>().TakeDamage(recallDamage);
            }
        }
    }


}
