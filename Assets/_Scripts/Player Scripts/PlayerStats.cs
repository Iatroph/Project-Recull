using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Slider healthSlider;
    public DamageEffect uiEffects;

    [Header("Debugging")]
    public bool godMode;

    private void Awake()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        currentHealth = maxHealth;
        healthText.text = "" + currentHealth;
        iFramesTimer = iFramesTime;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    TakeDamage(100);
        //}

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }

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
            uiEffects.RedFlash();
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
                if (GameManager.instance)
                {
                    GameManager.instance.DeathSequence();
                }
                Debug.Log("Player is Dead");
            }
            UpdateUI();

        }
    }

    public void UpdateUI()
    {
        healthText.text = "" + currentHealth;
        healthSlider.value = currentHealth;
    }

    public void Heal(float amount)
    {
        if(currentHealth < maxHealth)
        {
            if(currentHealth + amount > maxHealth)
            {
                amount = maxHealth - currentHealth;
            }

            uiEffects.GreenFlash();
            currentHealth += amount;
            UpdateUI();
        }
    }

    public void Die()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90f);
        GetComponent<PlayerCamera>().TweenCameraRotation(new Vector3(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y,-90));
        currentHealth = 0;
        godMode = true;
    }

}
