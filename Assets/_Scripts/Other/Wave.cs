using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public EnemySpawn[] spawnLocations;
    public List<GameObject> enemies;

    public bool waveActivated;
    public bool waveEnded;

    private void Update()
    {
        if (enemies.Count > 0)
        {
            foreach (GameObject g in enemies)
            {
                if (g == null)
                {
                    enemies.Remove(g);
                    if (enemies.Count == 0)
                    {
                        waveEnded = true;
                    }
                    break;
                }
            }
        }
    }

    public void Activate()
    {
        waveActivated = true;
        foreach (EnemySpawn es in spawnLocations)
        {
            StartCoroutine(Delay(es));
        }
    }

    public IEnumerator Delay(EnemySpawn es)
    {
        yield return new WaitForSeconds(es.spawnDelay);
        enemies.Add(es.Spawn());
    }
}
