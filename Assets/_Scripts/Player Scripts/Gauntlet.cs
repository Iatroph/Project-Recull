using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [Header("Rerferences")]
    public Transform blastPosition;

    [Header("Repulse Parameters")]
    public float repulseRadius;
    public float repulseKnockback;
    public float repulseUpwardKnockback;
    public float repulseDamage;

    [Header("Layermask")]
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RepulseBlast();
        }
    }

    public void RepulseBlast()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, repulseRadius, whatIsEnemy);

        foreach (Collider c in hitEnemies)
        {
            EnemyBase eb = c.GetComponent<EnemyBase>();
            if (eb.usesNavMeshAgent)
            {
                //eb.ToggleUpdatePosition(false);
                Vector3 dir = (c.transform.position - transform.position).normalized;
                Rigidbody rb = c.GetComponent<Rigidbody>();
                rb.AddForce(dir * repulseKnockback, ForceMode.Impulse);
                rb.AddForce(transform.up * repulseUpwardKnockback, ForceMode.Impulse);
            }
            RangedEnemy re = c.GetComponent<RangedEnemy>();
            re.DisabledGroundCheck();
            //c.transform.GetComponent<IDamageable>().TakeDamage(repulseDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(blastPosition.position, repulseRadius);
    }
}
