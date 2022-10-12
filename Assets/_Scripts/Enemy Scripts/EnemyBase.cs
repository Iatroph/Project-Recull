using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IDamageable
{
    protected NavMeshAgent navAgent;

    [Header("Enemy Info")]
    public string enemyName;
    public int enemyID;
    public bool usesNavMeshAgent;

    [Header("Enemy Stats")]
    public float maxHealth;
    private float currentHealth;

    [Header("Debugging")]
    public TMP_Text healthText;
    public bool isDisabled;

    protected void Awake()
    {
        currentHealth = maxHealth;
        if (healthText)
        {
            healthText.text = "" + maxHealth;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log( gameObject.name + " Took " + damage + " damage");
        currentHealth -= damage;
        if (healthText)
        {
            healthText.text = "" + currentHealth;

        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Knockback(Vector3 dir, float force, float upForce)
    {

    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void ToggleAI()
    {
        isDisabled = !isDisabled;
    }

    public virtual void ToggleUpdatePosition(bool toggle)
    {
        navAgent.updatePosition = toggle;
    }
}
