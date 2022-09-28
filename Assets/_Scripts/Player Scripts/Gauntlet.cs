using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauntlet : MonoBehaviour
{
    bool isShielding;
    bool isParrying = false;
    bool canUseEnergy = true;
    float coolDownTimer;
    float currentEnergy;

    [Header("References")]
    public Transform blastPosition;
    public Transform parryPosition;
    public Animator anim;
    public GameObject gauntletShield;
    public GameObject reflectedProjectile;
    public Animator shieldAnim;

    [Header("UI")]
    public Slider energySlider;

    [Header("Energy Parameters")]
    public float maxEnergy;
    public float regenRate;
    public float regenDelay;
    public float repulseCost;
    public float shieldHoldCost;
    public float coolDown;

    [Header("Repulse Parameters")]
    public float repulseRadius;
    public float repulseKnockback;
    public float repulseUpwardKnockback;
    public float repulseDamage;

    [Header("Parry Parameters")]
    public float parryRadius;
    public float parryDamageMultiplier;

    [Header("Layermask")]
    public LayerMask whatIsEnemy;
    public LayerMask whatIsProjectile;

    private void Awake()
    {
        gauntletShield.SetActive(false);
        currentEnergy = maxEnergy;
        energySlider.maxValue = maxEnergy;
        coolDownTimer = coolDown;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateEnergySlider()
    {
        energySlider.value = currentEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnergySlider();

        if (Input.GetKeyDown(KeyCode.F) && !isShielding && canUseEnergy && !isParrying)
        {
            if(currentEnergy >= repulseCost)
            {
                RepulseBlast();
                currentEnergy -= repulseCost;
            }
        }

        if(Input.GetMouseButtonDown(1) && currentEnergy > 0 && canUseEnergy)
        {
            anim.SetTrigger("StartShield");
        }

        if (Input.GetMouseButton(1) && currentEnergy > 0 && canUseEnergy)
        {
            anim.SetBool("IsHolding", true);
            gauntletShield.SetActive(true);
            isShielding = true;
            currentEnergy -= shieldHoldCost * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.F) && isParrying == false)
            {
                isParrying = true;
                ShieldParry();
                shieldAnim.SetTrigger("Parry");
                anim.SetTrigger("Parry");
            }
        }
        else
        {
            if (!isParrying)
            {
                gauntletShield.SetActive(false);
                anim.SetBool("IsHolding", false);
                isShielding = false;
            }
        }

        if(currentEnergy <= 0)
        {
            SetIsParrying();
            canUseEnergy = false;

        }

        if (!canUseEnergy)
        {
            coolDownTimer -= Time.deltaTime;
            if(coolDownTimer <= 0)
            {
                coolDownTimer = coolDown;
                canUseEnergy = true;
            }
        }

        if(currentEnergy < maxEnergy && !isShielding)
        {
            currentEnergy += regenRate * Time.deltaTime;
        }
    }

    public void SetIsParrying()
    {
        isParrying = false;
    }

    public void RepulseBlast()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(blastPosition.position, repulseRadius, whatIsEnemy);
        anim.SetTrigger("Repulse");
        foreach (Collider c in hitEnemies)
        {
            EnemyBase eb = c.GetComponent<EnemyBase>();
            if (eb.usesNavMeshAgent)
            {
                Vector3 dir = (c.transform.position - transform.position).normalized;
                eb.Knockback(dir, repulseKnockback, repulseUpwardKnockback);
            }
            c.transform.GetComponent<IDamageable>().TakeDamage(repulseDamage);
        }

    }
    
    public void ShieldParry()
    {
        Collider[] hitProjectiles = Physics.OverlapSphere(parryPosition.position, parryRadius, whatIsProjectile);
        foreach(Collider c in hitProjectiles)
        {
            GameObject proj = Instantiate(reflectedProjectile, parryPosition.position, parryPosition.rotation);
            proj.GetComponent<ReflectedProjectile>().damage = c.gameObject.GetComponent<EnemyProjectile>().damage * parryDamageMultiplier;
            Destroy(c.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(blastPosition.position, repulseRadius);
    }
}
