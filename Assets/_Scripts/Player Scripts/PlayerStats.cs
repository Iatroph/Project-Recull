using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    private float iFramesTimer;
    private bool iFramesOn;

    [HideInInspector]
    public float currentHealth;

    [Header("Player Parameters")]
    public float maxHealth;
    public float iFramesTime;

    [Header("UI")]
    public TMP_Text healthText;
    public DamageEffect de;

    [Header("Debugging")]
    public bool godMode;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
        iFramesTimer = iFramesTime;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    TakeDamage(10);
        //}

        IFrameTimer();
    }

    public void IFrameTimer()
    {
        if (iFramesOn)
        {
            iFramesTimer -= Time.deltaTime;
            if(iFramesTimer <= 0)
            {
                iFramesOn = false;
                iFramesTimer = iFramesTime;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0 && !iFramesOn && !godMode)
        {
            iFramesOn = true;
            currentHealth -= damage;
            de.RedFlash();
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log("Player is Dead");
            }
            healthText.text = "Health: " + currentHealth;

        }
    }

}
