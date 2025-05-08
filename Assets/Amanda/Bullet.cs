using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    
    void OnCollisionEnter(Collision collision)
    {
        TargetHealth health = collision.gameObject.GetComponent<TargetHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

public class TargetHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}