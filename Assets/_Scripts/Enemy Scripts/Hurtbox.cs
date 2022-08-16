using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    EnemyBase enemybase;

    [Header("Hurtbox Parameters")]
    public float damageMultiplier;
    public bool isWeakPoint;

    private void Awake()
    {
        enemybase = GetComponentInParent<EnemyBase>();
    }

    public void AdjustDamage(float damage)
    {
        enemybase.TakeDamage(damage * damageMultiplier);
    }
}
