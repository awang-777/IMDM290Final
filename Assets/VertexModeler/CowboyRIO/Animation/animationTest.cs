using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationTester : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        // Get the Animator component from your character
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        
        if (keyboard == null)
            return;
            
        // Set default state (Idle) when S is pressed
        if (keyboard.sKey.isPressed)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isCrouching", false);
        }
        
        // Trigger Walk animation when W is pressed
        if (keyboard.wKey.isPressed)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isCrouching", false);
        }
        
        // Trigger Crouch animation when D is pressed
        if (keyboard.dKey.isPressed)
        {
            animator.SetBool("isCrouching", true);
            animator.SetBool("isWalking", false);
        }
    }
}