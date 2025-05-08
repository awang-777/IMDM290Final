using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        
        // Check if this is an enemy bullet hitting the player
        if (gameObject.CompareTag("EnemyBullet") && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy bullet!");
            // Player damage logic here
        }
        // Check if this is a player bullet hitting an enemy
        else
        {
            TargetHealth health = collision.gameObject.GetComponent<TargetHealth>();
            if (health != null)
            {
                Debug.Log("Hit object with TargetHealth, applying damage");
                health.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }
}