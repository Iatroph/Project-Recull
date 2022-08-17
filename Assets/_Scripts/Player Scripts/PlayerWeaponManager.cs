using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public WeaponBase[] weaponArray = new WeaponBase[3];
    private WeaponBase currentWeapon;
    private int currentWeaponIndex;

    [Header("Input")]
    public KeyCode shootButton = KeyCode.Mouse0;
    public KeyCode recallKey = KeyCode.R;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = weaponArray[0];
        currentWeaponIndex = 0;


    }

    // Update is called once per frame
    void Update()
    {
        InputManager();

        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            currentWeaponIndex--;
            currentWeapon.gameObject.SetActive(false);
            if(currentWeaponIndex < 0)
            {
                currentWeaponIndex = weaponArray.Length - 1;
            }

            currentWeapon = weaponArray[currentWeaponIndex];
            currentWeapon.gameObject.SetActive(true);

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) // backward
        {
            currentWeaponIndex++;
            currentWeapon.gameObject.SetActive(false);
            if (currentWeaponIndex > weaponArray.Length - 1)
            {
                currentWeaponIndex = 0;
            }

            currentWeapon = weaponArray[currentWeaponIndex];
            currentWeapon.gameObject.SetActive(true);
        }

    }

    public void InputManager()
    {
        if (Input.GetKey(shootButton))
        {
            currentWeapon.ShootFromInput();
        }

        if (Input.GetKeyDown(recallKey))
        {
            currentWeapon.Recall();
        }
    }
}
