using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject weaponPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player picked up " + name);
            other.gameObject.GetComponent<PlayerWeaponManager>().EquipWeaponDirectly(weaponPrefab);
            gameObject.SetActive(false);
        }
    }
}
