using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemyAI : MonoBehaviour
{
    public List<EnemyBase> enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (EnemyBase eb in enemies)
            {
                eb.ToggleAI();
            }

            gameObject.SetActive(false);
        }
    }
}
