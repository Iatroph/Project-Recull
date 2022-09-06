using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector]
    public float currentHealth;

    [Header("Player Parameters")]
    public float maxHealth;

    [Header("UI")]
    public TMP_Text healthText;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log("Player is Dead");
            }
            healthText.text = "Health: " + currentHealth;

        }
    }

}
