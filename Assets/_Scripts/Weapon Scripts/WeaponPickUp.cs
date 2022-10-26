using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject weaponPrefab;

    public UnityEvent onPickUp;

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

            onPickUp.Invoke();

            //if(SceneManager.GetActiveScene().buildIndex == 1)
            //{
            //    IntroSequence.instance.EnableAmmoCounter();
            //}
        }
    }
}
