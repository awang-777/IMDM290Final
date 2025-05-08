using UnityEngine;

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

        if (gameObject.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddElimination();
            }
        }
        
        EnemyController enemyController = GetComponent<EnemyController>();
        if (enemyController != null)
        {

            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.gray;
            }

            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            
            enemyController.MarkAsDead();
            

            Destroy(gameObject, 2f);
        }
        else
        {

            Destroy(gameObject);
        }
    }
}