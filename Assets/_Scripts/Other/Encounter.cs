using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Encounter : MonoBehaviour
{
    public EnemySpawn[] spawnLocations;
    public List<GameObject> enemies;
    private bool isTriggered = false;
    bool encounterFinished = false;
    bool encounterStarted = false;

    public UnityEvent encounterStart;
    public UnityEvent afterEncounter;

    private void Update()
    {
        if(enemies.Count > 0)
        {
            foreach(GameObject g in enemies)
            {
                if(g == null)
                {
                    enemies.Remove(g);
                    if(enemies.Count == 0)
                    {
                        encounterFinished = true;
                    }
                    break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if(isTriggered && encounterFinished)
        {
            afterEncounter.Invoke();
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
