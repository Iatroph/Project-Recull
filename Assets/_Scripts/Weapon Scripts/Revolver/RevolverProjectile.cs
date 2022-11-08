using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverProjectile : ProjectileBase
{
    float delayTime;
    private TrailRenderer tr;
    Transform markedEnemy = null;
    bool isSearching = false;

    [Header("References")]
    public GameObject bullerTracer;
    public GameObject bulletImpactParticle;
    public GameObject impactSparkParticle;
    public GameObject muzzleFlashParticle;
    public Transform muzzleFlashSpawn;

    [Header("Recall Parameters")]
    public float upForce;
    public float recallRange;
    public float recallDamage;

    [Header("Recall Coroutine Times")]
    public float riseTime;
    public float holdTime;
    public float lockOnTime;
    public float returnTime;


    [Header("Other")]
    public LayerMask whatIsHurtBox;
    public LayerMask recallIgnore;
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

    new void Update()
    {
        base.Update();

        if (isSearching)
        {
            CheckForEnemy();
        }

        if (markedEnemy != null && isSearching)
        {
            if (Physics.Raycast(transform.position, (markedEnemy.position - transform.position).normalized, out RaycastHit hit, recallRange, ~recallIgnore))
            {
                if(hit.transform.gameObject.GetComponent<Hurtbox>() != null)
                {
                    transform.LookAt(markedEnemy.position);
                    transform.Rotate(90, 0, 0);
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

        }

    }

    public override void ActivateRecallAbility()
    {
        if (!abilityTriggered)
        {
            tr.startColor = recallTrailColor;
            delayTime = Random.Range(0, 0.2f);
            StartCoroutine(Rebound());
            abilityTriggered = true;
        }
    }

    new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public void SetTracerLine(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void CheckForEnemy()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, recallRange, whatIsHurtBox);

        List<GameObject> weakPoints = new List<GameObject>();
        List<GameObject> regularPoints = new List<GameObject>();

        foreach (Collider c in hitEnemies)
        {
            if (c.GetComponent<Hurtbox>() != null)
            {
                Hurtbox hb = c.GetComponent<Hurtbox>();
                if (hb.isWeakPoint == true)
                {
                    weakPoints.Add(c.gameObject);
                }
                else
                {
                    regularPoints.Add(c.gameObject);
                }
            }
        }

        float dist;
        float storedDist = recallRange;

        foreach (GameObject g in weakPoints)
        {
            dist = Vector3.Distance(transform.position, g.transform.position);
            if (dist < storedDist)
            {
                storedDist = dist;
                markedEnemy = g.transform;
            }
        }

    }

    public IEnumerator Rebound()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
        delayTime = Random.Range(0, 0.1f);
        yield return new WaitForSeconds(delayTime);
        bulletBounce.Spin();
        yield return new WaitForSeconds(riseTime);
        rb.useGravity = false;
        yield return new WaitForSeconds(holdTime);
        isSearching = true;
        yield return new WaitForSeconds(lockOnTime);

        if (markedEnemy != null)
        {
            tr.enabled = false;
            isSearching = false;
            RaycastHit hit;
            Vector3 dir = (markedEnemy.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out hit, float.MaxValue, ~recallIgnore))
            {

                if (hit.transform.gameObject.GetComponent<Hurtbox>() != null)
                {
                    GameObject MuzzleFlash = Instantiate(muzzleFlashParticle, muzzleFlashSpawn.position, Quaternion.identity);

                    hit.transform.gameObject.GetComponent<Hurtbox>().AdjustDamage(recallDamage, true);

                    GameObject tracer = Instantiate(bullerTracer, hit.point, Quaternion.identity);
                    SetTracerLine(tracer.GetComponent<LineRenderer>(), transform.position, hit.point);

                    GameObject impact = Instantiate(bulletImpactParticle, hit.point, Quaternion.LookRotation(hit.normal));

                    GameObject spark = Instantiate(impactSparkParticle, hit.point, Quaternion.identity);

                    transform.position = hit.point;
                    markedEnemy = null;
                    bulletBounce.Bounce();
                }

            }
        }
        tr.enabled = true;
        rb.useGravity = true;
        yield return new WaitForSeconds(returnTime);
        bulletBounce.Spin();
        ReturnToPlayer();
    }
}
