using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleVRMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.0f; // Movement speed
    
    [Header("Input References")]
    public InputActionReference moveInputAction; 
    private Vector2 inputAxis;
    private CharacterController characterController;
    private Camera xrCamera;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
            characterController.center = new Vector3(0, 1.0f, 0);
            characterController.height = 2.0f;
            characterController.radius = 0.3f;
        }
        
        xrCamera = GetComponentInChildren<Camera>();

        moveInputAction.action.performed += OnMoveInput;
        moveInputAction.action.canceled += OnMoveInput;
        moveInputAction.action.Enable();
        
    }
    
    void OnDestroy()
    {

        if (moveInputAction != null)
        {
            moveInputAction.action.performed -= OnMoveInput;
            moveInputAction.action.canceled -= OnMoveInput;
            moveInputAction.action.Disable();
        }
    }
    
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        inputAxis = context.ReadValue<Vector2>();
    }
    
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (xrCamera == null || characterController == null)
            return;

        Vector3 movement = new Vector3(inputAxis.x, 0, inputAxis.y);
        Quaternion yRotation = Quaternion.Euler(0, xrCamera.transform.eulerAngles.y, 0);
        movement = yRotation * movement;
        movement *= moveSpeed * Time.deltaTime;
        

        if (!characterController.isGrounded)
        {
            movement.y = Physics.gravity.y * Time.deltaTime;
        }
        
        characterController.Move(movement);
    }
}