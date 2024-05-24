using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to be spawned
    public float spawnInterval = 5f; // Initial time interval between spawns
    public float spawnIntervalReduction = 0.1f; // The amount by which the interval is reduced
    public float minSpawnInterval = 1f; // Minimum possible interval between spawns
    public Transform[] spawnPoints; // Array of spawn points

    private float currentSpawnInterval;

    void Start()
    {
        currentSpawnInterval = spawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            // Spawn enemies based on the game timer from GameManager
            float gameTimer = GameManager.instance.GetGameTimer();
            int enemyCount = Mathf.FloorToInt(gameTimer / 10) + 1; // Adjust this formula as needed
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
            }

            // Decrease the spawn interval to increase difficulty over time
            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= spawnIntervalReduction;
            }
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("No spawn points assigned in the EnemySpawner script.");
        }
    }
}
