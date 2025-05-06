using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class BasicVRCharacter : MonoBehaviour
{
    public Animator characterAnimator;
    public Transform headBone;
    public string walkParameterName = "isWalking";
    public string crouchParameterName = "isCrouching";
    public float moveSpeed = 2.0f;
    private Transform cameraTransform;
    private InputDevice leftHand;
    private InputDevice rightHand;
    private int walkParamID;
    private int crouchParamID;
    
    void Start()
    {
        cameraTransform = Camera.main != null ? Camera.main.transform : null;

        if (characterAnimator == null)
            characterAnimator = GetComponent<Animator>();
        
        if (characterAnimator != null)
        {
            walkParamID = Animator.StringToHash(walkParameterName);
            crouchParamID = Animator.StringToHash(crouchParameterName);
        }

        FindControllers();
        AlignWithCamera();
    }
    
    void AlignWithCamera()
    {
        if (headBone != null && cameraTransform != null)
        {
            Vector3 offset = cameraTransform.position - headBone.position;
            offset.y = 0; // Keep the character at the same height
            transform.position += offset;
        }
    }
    
    void FindControllers()
    {
        List<InputDevice> leftHandDevices = new List<InputDevice>();
        List<InputDevice> rightHandDevices = new List<InputDevice>();
        
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller,
            leftHandDevices);
            
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller,
            rightHandDevices);
            
        if (leftHandDevices.Count > 0)
            leftHand = leftHandDevices[0];
            
        if (rightHandDevices.Count > 0)
            rightHand = rightHandDevices[0];
    }
    
    void Update()
    {
        // Re-initialize controllers if needed
        if (!leftHand.isValid || !rightHand.isValid)
        {
            FindControllers();
            return;
        }

        HandleMovement();
        HandleCrouch();
    }
    
    void HandleMovement()
    {
        if (cameraTransform == null || characterAnimator == null) return;

        if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 input))
        {
            bool isMoving = input.magnitude > 0.1f;
            
            if (isMoving)
            {
                Vector3 forward = cameraTransform.forward;
                Vector3 right = cameraTransform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                
                Vector3 direction = (forward * input.y + right * input.x).normalized;
                
                // moves and rotate character
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            

            characterAnimator.SetBool(walkParamID, isMoving); //sets animation state
        }
    }
    
    void HandleCrouch()
    {
        if (characterAnimator == null) return;
        
        bool isCrouching = false;
        
        // checks right thumbstick
        if (rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightStick))
            if (rightStick.y < -0.5f)
                isCrouching = true;
        
        // checks primary button
        if (rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton))
            if (primaryButton)
                isCrouching = true;
        
        // sets animation state
        characterAnimator.SetBool(crouchParamID, isCrouching);
    }
}