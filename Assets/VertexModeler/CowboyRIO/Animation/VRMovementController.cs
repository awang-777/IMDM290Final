using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class VRMovementController : MonoBehaviour
{
    // Reference to the animator component on your humanoid
    public Animator animator;
    
    // Animation state names from Mixamo
    private const string IDLE_ANIMATION = "Idle";
    private const string WALK_ANIMATION = "Walk";
    private const string CROUCH_ANIMATION = "Crouch";
    
    // Movement thresholds
    public float movementThreshold = 0.1f;
    public float crouchThreshold = 0.5f;
    
    // VR input devices
    private List<InputDevice> leftHandDevices = new List<InputDevice>();
    private List<InputDevice> rightHandDevices = new List<InputDevice>();
    private List<InputDevice> headsetDevices = new List<InputDevice>();
    
    // Player starting height (for crouch detection)
    private float initialHeadHeight;
    
    // Animation parameter IDs (for efficiency)
    private int isWalkingID;
    private int isCrouchingID;
    
    void Start()
    {
        // Get the animator component if not set in inspector
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("No Animator component found on this GameObject!");
                return;
            }
        }
        
        // Cache animation parameter IDs
        isWalkingID = Animator.StringToHash("IsWalking");
        isCrouchingID = Animator.StringToHash("IsCrouching");
        
        // Get VR devices
        InitializeXRDevices();
        
        // Record initial head height for crouch detection
        InputDevice headset = headsetDevices.Count > 0 ? headsetDevices[0] : default;
        if (headset.isValid && headset.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headPosition))
        {
            initialHeadHeight = headPosition.y;
        }
    }
    
    void InitializeXRDevices()
    {
        // Get all VR input devices
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, leftHandDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, rightHandDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, headsetDevices);
    }
    
    void Update()
    {
        // If we don't have devices yet, try to get them
        if (leftHandDevices.Count == 0 || rightHandDevices.Count == 0 || headsetDevices.Count == 0)
        {
            InitializeXRDevices();
            return;
        }
        
        // Get primary controller for movement (can use left or right based on preference)
        InputDevice primaryController = leftHandDevices[0];
        InputDevice headset = headsetDevices[0];
        
        if (!primaryController.isValid || !headset.isValid)
            return;
            
        // Check for lateral movement (walking)
        bool isWalking = false;
        if (primaryController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement) || 
            primaryController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
        {
            // Either joystick input or controller velocity can be used
            float movementMagnitude = movement.magnitude > 0 ? movement.magnitude : velocity.magnitude;
            isWalking = movementMagnitude > movementThreshold;
        }
        
        // Check for crouch
        bool isCrouching = false;
        if (headset.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headPosition))
        {
            float currentHeight = headPosition.y;
            float heightDifference = initialHeadHeight - currentHeight;
            isCrouching = heightDifference > crouchThreshold;
        }
        
        // Apply animations based on movement detection
        animator.SetBool(isWalkingID, isWalking);
        animator.SetBool(isCrouchingID, isCrouching);
    }
}