using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public float waveSpawnDelay;

    public Wave[] waves;
    bool isTriggered;
    public UnityEvent afterEncounter;

    public IEnumerator SpawnWaves()
    {
        foreach(Wave wave in waves)
        {
            yield return new WaitForSecondsRealtime(waveSpawnDelay);
            wave.Activate();
            while (!wave.waveEnded)
            {
                yield return null;
            }
        }

        yield return new WaitForSecondsRealtime(0.5f);
        afterEncounter.Invoke();
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isTriggered == false)
        {
            isTriggered = true;
            StartCoroutine(SpawnWaves());
        }
    }
}
