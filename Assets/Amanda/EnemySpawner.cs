using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public int maxEnemies = 5;
    public float spawnInterval = 10f;
    public float minSpawnDistance = 15f;
    public float maxSpawnDistance = 30f;
    
    private int currentEnemies = 0;
    
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        StartCoroutine(SpawnEnemies());
    }
    
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            if (currentEnemies < maxEnemies && player != null)
            {
                SpawnEnemy();
            }
        }
    }
    
    void SpawnEnemy()
    {
        // Calculate random spawn position around player
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        
        Vector3 spawnPos = player.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        spawnPos.y = 1f; // Adjust height as needed
        
        // Spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        
        // Set player reference
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.player = player;
        }
        
        currentEnemies++;
        
        // Listen for destruction to update count
        StartCoroutine(MonitorEnemy(enemy));
    }
    
    IEnumerator MonitorEnemy(GameObject enemy)
    {
        while (enemy != null)
        {
            yield return new WaitForSeconds(1f);
        }
        
        // Enemy destroyed
        currentEnemies--;
    }
}