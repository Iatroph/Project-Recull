using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeaponManager : MonoBehaviour
{
    public WeaponBase[] weaponArray = new WeaponBase[3];
    private WeaponBase currentWeapon;
    private int currentWeaponIndex;

    [Header("UI")]
    public TMP_Text ammoText;
    public TMP_Text recallText;

    [Header("Input")]
    public KeyCode shootButton = KeyCode.Mouse0;
    public KeyCode recallKey = KeyCode.R;


    // Start is called before the first frame update
    void Start()
    {
        //currentWeapon = weaponArray[0];
        //currentWeaponIndex = 0;

        for (int i = 0; i < weaponArray.Length; i++)
        {
            //if (weaponArray[i].gameObject.activeSelf)
            //{
            //    currentWeapon = weaponArray[i];
            //    currentWeaponIndex = i;
            //    break;
            //}

            if (weaponArray[i].isActiveWeapon)
            {
                currentWeapon = weaponArray[i];
                currentWeaponIndex = i;
                break;
            }
        }

        //for (int i = 0; i < weaponArray.Length; i++)
        //{
        //    if (i != currentWeaponIndex)
        //    {
        //        weaponArray[i].gameObject.SetActive(false);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        UpdateAmmo();
        UpdateRecallTimer();
        Debug.Log(currentWeaponIndex);
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            currentWeaponIndex--;
            //currentWeapon.gameObject.SetActive(false);
            currentWeapon.ToggleMesh();

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weaponArray.Length - 1;
            }
            
            if(weaponArray[currentWeaponIndex] == null)
            {
                while (weaponArray[currentWeaponIndex] == null)
                {
                    currentWeaponIndex--;
                    if (currentWeaponIndex < 0)
                    {
                        currentWeaponIndex = weaponArray.Length - 1;
                        break;
                    }
                }
            }

            //if(currentWeaponIndex < 0)
            //{
            //    currentWeaponIndex = weaponArray.Length - 1;
            //}

            currentWeapon = weaponArray[currentWeaponIndex];
            //currentWeapon.gameObject.SetActive(true);
            currentWeapon.ToggleMesh();
            currentWeapon.PlaySwitchAnimation();

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) // backward
        {
            currentWeaponIndex++;
            //currentWeapon.gameObject.SetActive(false);
            currentWeapon.ToggleMesh();

            if (currentWeaponIndex > weaponArray.Length - 1)
            {
                currentWeaponIndex = 0;
            }
            else if (weaponArray[currentWeaponIndex] == null)
            {
                while (weaponArray[currentWeaponIndex] == null)
                {
                    currentWeaponIndex++;
                    if (currentWeaponIndex > weaponArray.Length - 1)
                    {
                        currentWeaponIndex = 0;
                        break;
                    }
                }
            }

            //if (currentWeaponIndex > weaponArray.Length - 1)
            //{
            //    currentWeaponIndex = 0;
            //}

            currentWeapon = weaponArray[currentWeaponIndex];
            //currentWeapon.gameObject.SetActive(true);
            currentWeapon.ToggleMesh();
            currentWeapon.PlaySwitchAnimation();

        }

    }

    public void InputManager()
    {
        if (Input.GetKey(shootButton))
        {
            currentWeapon.ShootFromInput();
        }

        if (Input.GetKeyDown(recallKey) && currentWeapon.canRecall)
        {
            currentWeapon.Recall();
        }
    }

    public void UpdateAmmo()
    {
        if(ammoText != null && currentWeapon != null)
        {
            ammoText.text = currentWeapon.currentAmmo + "/" + currentWeapon.magCapacity;
        }
    }
    public void UpdateRecallTimer()
    {
        if (recallText != null && currentWeapon != null)
        {
            if (currentWeapon.canRecall)
            {
                recallText.text = "READY!";
            }
            else
            {
                recallText.text = currentWeapon.recallCooldownTimer.ToString("#.00");

            }
        }
    }

    public void ReloadAmmo(int ammoID)
    {
        foreach(WeaponBase weapon in weaponArray)
        {
            if(ammoID == weapon.weaponID)
            {
                weapon.AddAmmo();
            }
        }
    }
}
