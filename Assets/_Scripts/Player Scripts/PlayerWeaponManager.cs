using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeaponManager : MonoBehaviour
{
    public WeaponBase[] weaponArray = new WeaponBase[3];
    [SerializeField] private WeaponBase currentWeapon;
    [SerializeField] private int currentWeaponIndex;
    [SerializeField] private int numberOfWeapons;

    private float globalRecallTimer;

    [Header("References")]
    public GameObject gunHolder;

    [Header("Global Recall")]
    public float globalRecallTime;

    [Header("UI")]
    public TMP_Text ammoText;
    public TMP_Text recallText;

    [Header("Input")]
    public KeyCode shootButton = KeyCode.Mouse0;
    public KeyCode recallKey = KeyCode.R;


    private void Awake()
    {
        numberOfWeapons = 0;
        globalRecallTimer = globalRecallTime;
        foreach(WeaponBase weapon in weaponArray)
        {
            if(weapon != null)
            {
                numberOfWeapons++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weaponArray.Length; i++)
        {
            if(weaponArray[i] != null)
            {
                if (weaponArray[i].isActiveWeapon)
                {
                    currentWeapon = weaponArray[i];
                    currentWeaponIndex = i;
                }
                else
                {
                    weaponArray[i].DisableMesh();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        UpdateAmmo();
        UpdateRecallTimer();
        if(numberOfWeapons > 1)
        {
            MouseWheelWeaponSwap();

        }

    }

    public void AddWeapon()
    {
        numberOfWeapons++;
    }

    public void MouseWheelWeaponSwap()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentWeapon != null) // forward
        {
            currentWeaponIndex--;
            currentWeapon.ToggleMesh();

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weaponArray.Length - 1;
            }

            if (weaponArray[currentWeaponIndex] == null)
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

            currentWeapon = weaponArray[currentWeaponIndex];
            currentWeapon.ToggleMesh();
            currentWeapon.PlaySwitchAnimation();

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentWeapon != null) // backward
        {
            currentWeaponIndex++;
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
            currentWeapon = weaponArray[currentWeaponIndex];
            currentWeapon.ToggleMesh();
            currentWeapon.PlaySwitchAnimation();

        }
    }

    public void InputManager()
    {
        if (Input.GetKey(shootButton) && currentWeapon != null)
        {
            currentWeapon.ShootFromInput();
        }

        if (Input.GetKey(recallKey) && currentWeapon != null)
        {
            globalRecallTimer -= Time.deltaTime;
            if (globalRecallTimer <= 0)
            {
                foreach (WeaponBase weapon in weaponArray)
                {
                    if (weapon != null && weapon.canRecall)
                    {
                        weapon.Recall();
                    }
                }
                globalRecallTimer = globalRecallTime;
            }

        }
        else if (Input.GetKeyUp(recallKey) && currentWeapon != null)
        {
            if (currentWeapon.canRecall)
            {
                currentWeapon.Recall();
            }
            globalRecallTimer = globalRecallTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NumSwapWeapons(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NumSwapWeapons(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NumSwapWeapons(2);
        }
    }

    public void NumSwapWeapons(int index)
    {
        if(weaponArray.Length == 0)
        {
            return;
        }
        else if(weaponArray[index] == null || currentWeaponIndex == index)
        {
            return;
        }
        else
        {
            if(weaponArray[index] != null)
            {
                currentWeapon.ToggleMesh();
                currentWeaponIndex = index;
                currentWeapon = weaponArray[index];
                currentWeapon.ToggleMesh();
                currentWeapon.PlaySwitchAnimation();
            }
        }
    }

    public void EquipWeaponDirectly(GameObject weapon)
    {
        for(int i = 0; i < weaponArray.Length; i++)
        {
            if(weaponArray[i] == null)
            {
                GameObject gun = Instantiate(weapon, gunHolder.transform.position, Quaternion.identity);
                gun.name = weapon.name;
                gun.transform.SetParent(gunHolder.transform);
                currentWeaponIndex = i;
                weaponArray[i] = gun.GetComponent<WeaponBase>();
                currentWeapon = weaponArray[currentWeaponIndex];
                currentWeapon.PlaySwitchAnimation();
                AddWeapon();
                return;

            }
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
            if(weapon != null)
            {
                if (ammoID == weapon.weaponID)
                {
                    weapon.AddAmmo();
                }
            }
        }
    }
}
