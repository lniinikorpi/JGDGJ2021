using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn;
    public bool enemyAlive;
    float respawnTime;
    float timer;

    private void Update()
    {
        if(!enemyAlive)
        {
            if (Time.timeScale > 0)
            {
                timer += Time.deltaTime;
                if(timer >= respawnTime)
                {
                    SpawnEnemy();
                    timer = 0;
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemyToSpawn = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count - 1)];
        enemyToSpawn.transform.position = transform.position;
        enemyToSpawn.SetActive(true);
        enemyToSpawn.GetComponent<EnemyBase>().InitEnemy();
        enemyToSpawn.GetComponent<EnemyBase>().spawn = this;
        enemyAlive = true;
        respawnTime = Random.Range(GameManager.instance.minEnemyRespawnTime, GameManager.instance.maxEnemyRespawnTime);
    }
}
