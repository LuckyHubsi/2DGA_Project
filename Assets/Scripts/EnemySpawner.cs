using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float initialSpawnInterval = 5f; // Initial interval between spawns
    public bool facingLeft = true; // Determines the initial facing direction of the spawned enemies
    public float initialMoveSpeed = 1f; // Initial move speed for enemies
    public float randomDelayRange = 1f; // Maximum range for random delay

    private float spawnInterval;
    private float nextSpawnTime = 0f;

    void Start()
    {
        spawnInterval = initialSpawnInterval;
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        // Decrease spawn interval based on game time
        spawnInterval = Mathf.Max(initialSpawnInterval / (1 + Time.time * 0.1f), 1f);

        // Add a random delay to the next spawn time
        float randomDelay = Random.Range(0, randomDelayRange);
        nextSpawnTime = Time.time + spawnInterval + randomDelay;
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        // Select a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Instantiate the enemy at the selected spawn point
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Set the enemy's initial facing direction and move speed
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetFacingDirection(facingLeft);
            enemyScript.moveSpeed = initialMoveSpeed * (1 + Time.time * 0.01f); // Increase move speed based on game time
        }
    }
}
