using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 100f;
    public float moveSpeed = 0f; // Set to 0 for stationary enemies
    
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform gunPosition;
    public Transform player;
    public float bulletSpeed = 15f;
    public float minShootInterval = 3f;
    public float maxShootInterval = 7f;
    public float shootingAccuracy = 0.9f; // 1.0 is perfect accuracy, lower values add randomness
    
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
        
        // Get or add TargetHealth component
        targetHealth = GetComponent<TargetHealth>();
        if (targetHealth == null)
        {
            targetHealth = gameObject.AddComponent<TargetHealth>();
        }
        
        // Set the health in TargetHealth to match our health
        targetHealth.maxHealth = health;
        
        // Start shooting routine
        StartCoroutine(ShootAtPlayer());
    }
    
    void Update()
    {
        if (isDead)
            return;
            
        // Always face the player
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // Keep enemy upright
            
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }
    
    IEnumerator ShootAtPlayer()
    {
        while (!isDead)
        {
            // Wait random time between shots
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
        if (bulletPrefab == null || gunPosition == null)
            return;
            
        // Calculate direction to player with some inaccuracy
        Vector3 directionToPlayer = player.position - gunPosition.position;
        
        // Add randomness based on accuracy
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
        
        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, gunPosition.position, Quaternion.LookRotation(directionToPlayer));
        
        // Add enemy tag to bullet so player can distinguish enemy bullets
        bullet.tag = "EnemyBullet";
        
        // Make enemy bullets look different (optional)
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.material.color = Color.red;
        }
        
        // Set velocity - UPDATED to use linearVelocity instead of velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = directionToPlayer * bulletSpeed;
        }
        
        // Destroy bullet after 5 seconds
        Destroy(bullet, 5f);
    }
    
    // Listen for death events from TargetHealth to update our state
    void OnEnable()
    {
        // Subscribe to health changes if needed in the future
    }
    
    void OnDisable()
    {
        // Unsubscribe if needed
    }
    
    // This function can be called to immediately mark as dead (e.g., from TargetHealth)
    public void MarkAsDead()
    {
        isDead = true;
    }
}