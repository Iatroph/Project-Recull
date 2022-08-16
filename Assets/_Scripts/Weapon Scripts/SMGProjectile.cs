using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGProjectile : ProjectileBase
{
    private float tickTimer;
    private Collider collidr;
    private TrailRenderer tr;

    [Header("Recall Parameters")]
    public float recallDamage;
    public float sawRadius;
    public float tickTime;

    [Header("Other")]
    public Color recallTrailColor;

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

    }

    public override void ActivateRecallAbility()
    {
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
