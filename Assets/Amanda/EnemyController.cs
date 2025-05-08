using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 100f;
    public float moveSpeed = 0f;
    
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform gunPosition;
    public Transform player;
    public float bulletSpeed = 15f;
    public float minShootInterval = 3f;
    public float maxShootInterval = 7f;
    public float shootingAccuracy = 0.9f;
    
    private bool isDead = false;
    private TargetHealth targetHealth;
    
    void Start()
    {
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        // Setup target health
        targetHealth = GetComponent<TargetHealth>();
        if (targetHealth == null)
        {
            targetHealth = gameObject.AddComponent<TargetHealth>();
        }
        targetHealth.maxHealth = health;
        
        // Setup renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(renderer.material);
        }
        
        // Start shooting coroutine
        StartCoroutine(ShootAtPlayer());
    }
    
    // Update method removed entirely since we don't want rotation updates
    
    IEnumerator ShootAtPlayer()
    {
        while (!isDead)
        {
            float waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);
            
            if (player != null)
            {
                FireAtPlayer();
            }
        }
    }
    
    void FireAtPlayer()
    {
        if (bulletPrefab == null || gunPosition == null || player == null)
            return;
            
        Vector3 directionToPlayer = player.position - gunPosition.position;
        
        if (shootingAccuracy < 1.0f)
        {
            float randomFactor = 1.0f - shootingAccuracy;
            directionToPlayer += new Vector3(
                Random.Range(-randomFactor, randomFactor),
                Random.Range(-randomFactor, randomFactor),
                Random.Range(-randomFactor, randomFactor)
            );
        }
        
        directionToPlayer.Normalize();
        
        // Create the bullet with the direction to player, but don't rotate the enemy
        GameObject bullet = Instantiate(bulletPrefab, gunPosition.position, Quaternion.LookRotation(directionToPlayer));
        bullet.tag = "EnemyBullet";
        
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.material.color = Color.red;
        }
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = directionToPlayer * bulletSpeed;
        }
        
        Destroy(bullet, 5f);
    }
    
    public void MarkAsDead()
    {
        isDead = true;
    }
}