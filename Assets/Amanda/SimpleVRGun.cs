using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleVRGun : MonoBehaviour
{
    [Header("References")]
    public GameObject gunPrefab; 
    public GameObject bulletPrefab; 
    public Transform bulletSpawnPoint; 
    
    [Header("Gun Positioning")]
    public Vector3 gunPosition = new Vector3(0.2f, -0.2f, 0.5f); 
    
    [Header("Bullet Settings")]
    public float bulletSpeed = 30f; 
    public float bulletLifetime = 5f;
    
    [Header("Input")]
    public InputActionReference shootAction;
    
    private GameObject gun;
    private GameObject crosshair;
    
    void Start()
    {
        gun = Instantiate(gunPrefab, transform);
        gun.transform.localPosition = gunPosition;
        gun.transform.localRotation = Quaternion.identity;
        
        CreateCrosshair();
        
        if (shootAction != null)
        {
            shootAction.action.performed += OnShoot;
            shootAction.action.Enable();
        }
        
        if (bulletSpawnPoint == null && gun != null)
        {
            GameObject spawnPoint = new GameObject("BulletSpawnPoint");
            spawnPoint.transform.parent = gun.transform;
            spawnPoint.transform.localPosition = new Vector3(0, 0, 0.3f);
            bulletSpawnPoint = spawnPoint.transform;
        }
    }
    
    void OnDestroy()
    {
        if (shootAction != null)
        {
            shootAction.action.performed -= OnShoot;
        }
    }
    
    void CreateCrosshair()
    {
        crosshair = new GameObject("Crosshair");
        crosshair.transform.parent = transform;
        crosshair.transform.localPosition = new Vector3(0, 0, 1);
        
        SpriteRenderer renderer = crosshair.AddComponent<SpriteRenderer>();
        
        Texture2D tex = new Texture2D(16, 16);
        Color clear = new Color(0, 0, 0, 0);
        Color white = Color.white;
        
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                if (x == 8 || y == 8)
                    tex.SetPixel(x, y, white);
                else
                    tex.SetPixel(x, y, clear);
            }
        }
        tex.Apply();
        
        Sprite crosshairSprite = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        renderer.sprite = crosshairSprite;
        renderer.sortingOrder = 1000;
    }
    
    void OnShoot(InputAction.CallbackContext context)
    {
        FireBullet();
        
        Debug.Log("Shot fired!");
    }
    
    void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("No bullet prefab assigned!");
            return;
        }
        
        Vector3 spawnPos = bulletSpawnPoint != null 
            ? bulletSpawnPoint.position 
            : transform.position + transform.forward * 0.5f;
            
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        
        bullet.transform.forward = transform.forward;
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        
        rb.linearVelocity = transform.forward * bulletSpeed;
        
        Destroy(bullet, bulletLifetime);
    }
}