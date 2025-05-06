using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class BasicVRCharacter : MonoBehaviour
{
    // the character to animate
    public Animator characterAnimator;
    
    // movement settings
    public float moveSpeed = 2.0f;
    
    // head position 
    public Transform headBone;
    
    // main camera reference
    private Transform cameraTransform;
    
    // controller references
    private InputDevice leftHand;
    private InputDevice rightHand;
    
    // animation parameters
    private int walkParamID;
    private int crouchParamID;
    
    void Start()
    {
        // finds the main camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No main camera found!");
        }
        
        // intialize animation 
        if (characterAnimator != null)
        {
            walkParamID = Animator.StringToHash("isWalking");
            crouchParamID = Animator.StringToHash("isCrouching");
        }
        
        FindControllers();
        
        // Position the character correctly at start
        if (headBone != null && cameraTransform != null)
        {
            // Calculate how far to move the character to align with camera
            Vector3 offset = cameraTransform.position - headBone.position;
            offset.y = 0; // Keep the character at the same height
            
            // Move the entire character
            transform.position += offset;
        }
    }
    
    void FindControllers()
    {
        List<InputDevice> leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller,
            leftHandDevices);
            
        if (leftHandDevices.Count > 0)
        {
            leftHand = leftHandDevices[0];
        }
        
        List<InputDevice> rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller,
            rightHandDevices);
            
        if (rightHandDevices.Count > 0)
        {
            rightHand = rightHandDevices[0];
        }
    }
    
    void Update()
    {
        // If no controllers, try to find them
        if (!leftHand.isValid || !rightHand.isValid)
        {
            FindControllers();
            return;
        }
        
        // Handle controller input
        HandleMovement();
        HandleCrouch();
    }
    
    void HandleMovement()
    {
        if (cameraTransform == null) return;
        
        // Get joystick input from left controller
        Vector2 input = Vector2.zero;
        if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out input))
        {
            // Only move if input is significant
            if (input.magnitude > 0.1f)
            {
                // Get forward and right directions from camera (ignore y)
                Vector3 forward = cameraTransform.forward;
                Vector3 right = cameraTransform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                
                // calculate movement direction
                Vector3 direction = (forward * input.y + right * input.x).normalized;
                
                // moves the character
                transform.position += direction * moveSpeed * Time.deltaTime;
                
                // rotates to face movement direction
                transform.rotation = Quaternion.LookRotation(direction);
                
                // sets walking animation
                if (characterAnimator != null)
                {
                    characterAnimator.SetBool(walkParamID, true);
                }
            }
            else
            {
                // Set idle animation
                if (characterAnimator != null)
                {
                    characterAnimator.SetBool(walkParamID, false);
                }
            }
        }
    }
    
    void HandleCrouch()
    {
        if (characterAnimator == null) return;
        
        // checks for crouch input from right controller
        bool isCrouching = false;
        
        // option 1: Right thumbstick down
        Vector2 rightStick = Vector2.zero;
        if (rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
        {
            if (rightStick.y < -0.5f)
            {
                isCrouching = true;
            }
        }
        
        // option 2: Primary button
        bool primaryButton = false;
        if (rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButton))
        {
            if (primaryButton)
            {
                isCrouching = true;
            }
        }
        
        // sets crouch animation
        characterAnimator.SetBool(crouchParamID, isCrouching);
    }
}