using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleVRGun : MonoBehaviour
{
    [Header("References")]
    public GameObject gunPrefab;
    
    [Header("Gun Positioning")]
    public Vector3 gunPosition = new Vector3(0.2f, -0.2f, 0.5f); // Right, down, forward
    
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

        Debug.Log("Shot fired!");
        

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.transform.name);
            
        }
    }
}