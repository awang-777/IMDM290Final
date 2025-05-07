using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    // Name of your main game scene to load (set this in the Inspector)
    public string gameSceneName = "MainScene";
    
    // Optional delay before loading the scene (in seconds)
    public float loadDelay = 1.0f;
    
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
            Debug.Log("Start button collider set to trigger mode");
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
            Debug.Log("Player entered start button trigger - loading game scene: " + gameSceneName);
            
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
            
            // Load the game scene after optional delay
            Invoke("LoadGameScene", loadDelay);
        }
    }
    
    private void LoadGameScene()
    {
        // Make sure the scene exists in build settings
        if (SceneUtility.GetBuildIndexByScenePath(gameSceneName) >= 0)
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Scene '" + gameSceneName + "' is not in the build settings! Add it in File > Build Settings");
        }
    }
}