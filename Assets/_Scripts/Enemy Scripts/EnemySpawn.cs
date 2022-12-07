using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public float spawnDelay;
    public GameObject worldCube;

    public SoundFX spawnSound;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        worldCube.SetActive(false);
    }

    public GameObject Spawn()
    {
        source.volume = MyAudioManager.instance.GetSFXVolume();
        source.Play();
        //MyAudioManager.instance.PlaySoundAtPoint(spawnSound, transform.position);
        GameObject spawnedEnemy = Instantiate(enemy, transform.position, transform.rotation);
        spawnedEnemy.name = enemy.GetComponent<EnemyBase>().name;
        return spawnedEnemy;
    }

}
