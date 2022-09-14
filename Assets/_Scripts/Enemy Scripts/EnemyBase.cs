using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Enemy Info")]
    public string enemyName;
    public int enemyID;

    [Header("Enemy Stats")]
    public float maxHealth;
    public float currentHealth;

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

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void ToggleAI()
    {
        isDisabled = !isDisabled;
    }
}
