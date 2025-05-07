using UnityEngine;

public class QuitButtonScript : MonoBehaviour
{
    // Optional delay before quitting (in seconds)
    public float quitDelay = 1.0f;
    
    // Optional feedback elements
    public AudioSource activationSound;
    public GameObject visualEffect;
    
    // Prevent multiple triggers
    private bool hasBeenTriggered = false;
    
    private void Start()
    {
        // Make sure the collider is set to trigger
        Collider buttonCollider = GetComponent<Collider>();
        if (buttonCollider != null && !buttonCollider.isTrigger)
        {
            buttonCollider.isTrigger = true;
            Debug.Log("Quit button collider set to trigger mode");
        }
        
        // Hide visual effect at start if present
        if (visualEffect != null)
        {
            visualEffect.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Player entered quit button trigger - quitting application");
            
            // Play sound if available
            if (activationSound != null)
            {
                activationSound.Play();
            }
            
            // Show visual effect if available
            if (visualEffect != null)
            {
                visualEffect.SetActive(true);
            }
            
            // Quit the application after optional delay
            Invoke("QuitApplication", quitDelay);
        }
    }
    
    private void QuitApplication()
    {
        // This will only work in a built application
        #if UNITY_EDITOR
            // If we're in the editor, stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
            Debug.Log("Editor play mode stopped");
        #else
            // If we're in a build, quit the application
            Application.Quit();
            Debug.Log("Application has quit");
        #endif
    }
}