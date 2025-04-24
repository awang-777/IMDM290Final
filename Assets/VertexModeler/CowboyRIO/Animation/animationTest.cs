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
            
        // idle = s key
        if (keyboard.sKey.isPressed)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isCrouching", false);
        }
        
        // Trigger Walk = w key
        if (keyboard.wKey.isPressed)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isCrouching", false);
        }
        
        // crouch = d key
        if (keyboard.dKey.isPressed)
        {
            animator.SetBool("isCrouching", true);
            animator.SetBool("isWalking", false);
        }
    }
}