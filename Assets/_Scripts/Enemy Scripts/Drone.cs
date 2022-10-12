using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyBase
{
    GameObject player;
    private float distanceFromPlayer;
    private Vector3 directionToPlayer;

    bool playerInRange;

    bool isAttacking;

    float attackTimer;

    private float attackInterval;

    [Header("References")]
    public GameObject projectile;

    [Header("Particle Effects")]
    public GameObject deathExplosionParticle;

    [Header("Attack Parameters")]
    public float projectileDamage;
    public Transform projectileSpawn;
    public float initialAttackInterval;
    public float baseAttackInterval;
    public float attackIntervalModifier;

    new void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        attackInterval = initialAttackInterval;
        attackTimer = attackInterval;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        directionToPlayer = player.transform.position - transform.position;

        if (!isDisabled)
        {
            AttackingState();
            RotateTowardPlayer();
        }
    }

    public void AttackingState()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            isAttacking = true;
            GameObject proj = Instantiate(projectile, projectileSpawn.position, Quaternion.LookRotation(directionToPlayer));
            proj.GetComponent<EnemyProjectile>().damage = projectileDamage;
            attackInterval += Random.Range(-attackIntervalModifier, attackIntervalModifier);
            attackTimer = attackInterval;
            attackInterval = baseAttackInterval;
        }
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
}
