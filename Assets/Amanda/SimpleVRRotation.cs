using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleVRRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 60f; 
    
    [Header("Input")]
    public InputActionReference rightThumbstick; 
    private Vector2 lookInput;
    private Transform cameraOffset;
    
    void Start()
    {
        cameraOffset = transform.Find("Camera Offset");
        
        if (cameraOffset == null)
        {
            Debug.LogError("Camera Offset not found in XR Origin children!");
        }
        
        if (rightThumbstick != null)
        {
            rightThumbstick.action.performed += OnLookInput;
            rightThumbstick.action.canceled += OnLookInput;
            rightThumbstick.action.Enable();
        }
        else
        {
            Debug.LogError("Right thumbstick input not assigned!");
        }
    }
    
    void OnDestroy()
    {
        if (rightThumbstick != null)
        {
            rightThumbstick.action.performed -= OnLookInput;
            rightThumbstick.action.canceled -= OnLookInput;
        }
    }
    
    private void OnLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    
    void Update()
    {
        if (cameraOffset != null)
        {
            float rotation = lookInput.x * rotationSpeed * Time.deltaTime;
            cameraOffset.Rotate(0, rotation, 0);
        }
    }
}