using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectile : ProjectileBase
{
    private TrailRenderer tr;

    [Header("References")]
    public GameObject explosionParticle;
    public GameObject explosionImpactParticle;

    [Header("Recall Parameters")]
    public float explosionRadius;
    public float explosionDamage;

    [Header("Other")]
    public LayerMask whatIsEnemy;
    public Color recallTrailColor;

    new void Awake()
    {
        base.Awake();
        tr = GetComponent<TrailRenderer>();

    }

    new void Start()
    {
        base.Start();
    }

    public override void ActivateRecallAbility()
    {
        ReturnToPlayer();
        GameObject explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        tr.startColor = recallTrailColor;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius, whatIsEnemy);
        
        foreach (Collider c in hitEnemies)
        {
            c.transform.GetComponent<IDamageable>().TakeDamage(explosionDamage);
            Vector3 dir = (c.transform.position - transform.position).normalized;
            if(Physics.Raycast(transform.position, dir, out RaycastHit hit, explosionRadius, whatIsEnemy))
            {
                GameObject sparks = Instantiate(explosionImpactParticle, hit.transform.position, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

}
