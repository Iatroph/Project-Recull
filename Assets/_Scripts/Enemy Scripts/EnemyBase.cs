using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent navAgent;

    [Header("Enemy Info")]
    public string enemyName;
    public int enemyID;
    public bool usesNavMeshAgent;

    [Header("Enemy Stats")]
    public float maxHealth;
    private float currentHealth;

    [Header("References")]
    public GameObject healthPickup;
    public SoundFX deathSound;

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

    public virtual void TakeDamage(float damage, bool isRecallDamage)
    {
        Debug.Log( gameObject.name + " Took " + damage + " damage");

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0 && isRecallDamage)
            {
                SpawnHealthPickup();
                Die();
            }
            else if(currentHealth <= 0)
            {
                Die();
            }


            if (healthText)
            {
                healthText.text = "" + currentHealth;
            }
        }
    }

    public virtual void Knockback(Vector3 dir, float force, float upForce)
    {

    }

    public virtual void SpawnHealthPickup()
    {
        GameObject healthDrop = Instantiate(healthPickup, transform.position, Quaternion.identity);
    }

    public virtual void Die()
    {
        if (GameManager.instance)
        {
            GameManager.instance.IncreaseKillCount();
        }
        MyAudioManager.instance.PlaySoundAtPoint(deathSound, transform.position);
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
