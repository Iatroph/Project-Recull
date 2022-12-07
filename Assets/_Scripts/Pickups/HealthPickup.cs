using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount;
    private GameObject Player;
    public SoundFX healSound;

    private PlayerStats ps;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), Player.GetComponent<Collider>());
        ps = Player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet Collection") && ps.currentHealth < ps.maxHealth)
        {
            MyAudioManager.instance.PlaySoundOneShot(healSound);
            Player.GetComponent<PlayerStats>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
