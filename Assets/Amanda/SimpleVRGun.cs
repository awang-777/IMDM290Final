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
    
    [Header("Crosshair Settings")]
    public float crosshairDistance = 3f;
    public Vector2 crosshairOffset = new Vector2(0.015f, -0.015f);
    
    [Header("Input")]
    public InputActionReference shootAction;
    
    private GameObject gun;
    private GameObject crosshair;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
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
        
        UpdateCrosshairPosition();
    }
    
    void Update()
    {
        UpdateCrosshairPosition();
    }
    
    void UpdateCrosshairPosition()
    {
        if (crosshair == null || mainCamera == null)
            return;
            
        Vector3 position = transform.position + transform.forward * crosshairDistance;
        
        position += transform.right * crosshairOffset.x;
        position += transform.up * crosshairOffset.y;
        
        crosshair.transform.position = position;
        crosshair.transform.rotation = transform.rotation;
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
        
        Vector3 shootDirection = transform.forward;
        
        Vector3 spawnPos = bulletSpawnPoint != null
            ? bulletSpawnPoint.position
            : transform.position + shootDirection * 0.5f;
            
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.LookRotation(shootDirection));
        bullet.transform.forward = shootDirection;
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        
        rb.linearVelocity = shootDirection * bulletSpeed;
        
        Destroy(bullet, bulletLifetime);
    }
}