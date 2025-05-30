using TMPro;
using UnityEngine;
using UnityEngine.InputSystem; // NEW Input System namespace

public class Bullet_Travel : MonoBehaviour
{
    public Gun_rotation gunRotation;
    public Transform firePoint;
    public GameObject muzzleFlash;
    public float bulletSpeed = 20f;
    public int maxBullets = 7;

    public TextMeshProUGUI ammoCount;

    public AudioClip fireSound;
    public AudioClip reloadSound;

    private AudioSource audioSource;
    private GameObject bulletPrefab;
    private int currentBullets;
    private float reloadTimer;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.gun.fire.performed += OnFirePerformed;
    }

    void OnDisable()
    {
        inputActions.gun.fire.performed -= OnFirePerformed;
        inputActions.Disable();
    }

    void Start()
    {
        currentBullets = maxBullets;
        reloadTimer = 0f;

        if (gunRotation != null && gunRotation.cylinderRotation != null)
        {
            bulletPrefab = gunRotation.cylinderRotation.GetComponentInChildren<Transform>().gameObject;
            bulletPrefab.SetActive(false);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        updateAmmoUI();
    }

    void Update()
    {
        reloading();
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        fireBullet();
    }

    void fireBullet() {
        if (currentBullets <= 0) {
            Debug.Log("Out of Bullets! Reloading...");
            return;
        }

        gunRotation?.rotateCylinder();

        if (bulletPrefab != null && firePoint != null) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.SetActive(true);

            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            if (rigidbody != null) {
                rigidbody.linearVelocity = firePoint.forward * bulletSpeed;
            }

            TrailRenderer trail = bullet.GetComponent<TrailRenderer>();
            if (trail != null) {
                trail.Clear();
                trail.emitting = true;
            }

            Destroy(bullet, 5f);
        }

        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        showMuzzleFlash();
        currentBullets--;
        updateAmmoUI();
    }

    void showMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            Invoke("hideMuzzleFlash", 0.1f);
        }
    }

    void hideMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }

    void reloading()
    {
        if (currentBullets < maxBullets)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= 3f)
            {
                currentBullets++;
                reloadTimer = 0f;
                if (reloadSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(reloadSound);
                }
                Debug.Log("Reloaded one bullet. Current: " + currentBullets);
                updateAmmoUI();
            }
        }
    }

    void updateAmmoUI()
    {
        if (ammoCount != null)
        {
            ammoCount.text = $"Ammo: {currentBullets}/{maxBullets}";
        }
    }
}

