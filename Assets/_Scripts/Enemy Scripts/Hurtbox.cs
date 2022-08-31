using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    EnemyBase enemybase;

    [Header("Hurtbox Parameters")]
    public float damageMultiplier = 1;
    public bool isWeakPoint;

    private void Awake()
    {
        if (GetComponent<EnemyBase>())
        {
            enemybase = GetComponent<EnemyBase>();
        }
        else
        {
            enemybase = GetComponentInParent<EnemyBase>();

        }
    }

    public void AdjustDamage(float damage)
    {
        enemybase.TakeDamage(damage * damageMultiplier);
    }
}
