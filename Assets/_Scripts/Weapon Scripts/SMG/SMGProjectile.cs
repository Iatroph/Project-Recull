using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGProjectile : ProjectileBase
{
    private float tickTimer;
    private Collider collidr;
    private TrailRenderer tr;
    private GameObject markedEnemy;

    [Header("References")]
    public GameObject sawBlade;
    public GameObject casing;
    public GameObject bulletHead;
    public GameObject sawSpark;
    public GameObject smallSawSpark;

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
        sawBlade.SetActive(false);
    }

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        CheckForEnemy();
        base.Update();
        if (isReturning && markedEnemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, markedEnemy.transform.position, trackSpeed * Time.deltaTime);
        }

        if (isReturning)
        {
            bulletHead.GetComponent<QuickOutline>().enabled = false;
            casing.GetComponent<QuickOutline>().enabled = false;
            sawBlade.SetActive(true);
            sawBlade.transform.Rotate(0, 6 * 60 * Time.deltaTime, 0);
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
        ReturnToPlayer();
        collidr.GetComponent<CapsuleCollider>().radius = sawRadius;
        tr.startColor = recallTrailColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && isReturning)
        {
            //collision.transform.GetComponent<IDamageable>().TakeDamage(recallDamage);
            collision.transform.GetComponent<EnemyBase>().TakeDamage(recallDamage, true);
            GameObject spark = Instantiate(sawSpark, collision.GetContact(0).point, Quaternion.identity);
        }
        else
        {
            if (isReturning)
            {
                GameObject smallSpark = Instantiate(smallSawSpark, collision.GetContact(0).point, Quaternion.identity);
                bulletBounce.Bounce();
            }
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
                //collision.transform.GetComponent<IDamageable>().TakeDamage(recallDamage);
                collision.transform.GetComponent<EnemyBase>().TakeDamage(recallDamage, true);
                GameObject spark = Instantiate(sawSpark, collision.GetContact(0).point, Quaternion.identity);

            }
        }
    }


}
