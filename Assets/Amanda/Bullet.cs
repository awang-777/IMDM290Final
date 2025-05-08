using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        
        if (gameObject.CompareTag("EnemyBullet") && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy bullet!");
        }
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