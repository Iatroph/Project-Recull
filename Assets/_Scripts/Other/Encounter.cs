using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public EnemySpawn[] spawnLocations;
    public List<GameObject> enemies;
    private bool isTriggered = false;

    private void Update()
    {
        if(enemies.Count > 0)
        {
            foreach(GameObject g in enemies)
            {
                if(g == null)
                {
                    enemies.Remove(g);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isTriggered == false)
        {
            isTriggered = true;
            foreach(EnemySpawn es in spawnLocations)
            {
                StartCoroutine(Delay(es));
            }
        }
    }

    public IEnumerator Delay(EnemySpawn es)
    {
        yield return new WaitForSeconds(es.spawnDelay);
        enemies.Add(es.Spawn());
    }
}
