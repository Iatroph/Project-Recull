using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public float spawnDelay;


    public GameObject Spawn()
    {
        GameObject spawnedEnemy = Instantiate(enemy, transform.position, transform.rotation);
        spawnedEnemy.name = enemy.GetComponent<EnemyBase>().name;
        return spawnedEnemy;
    }

}
