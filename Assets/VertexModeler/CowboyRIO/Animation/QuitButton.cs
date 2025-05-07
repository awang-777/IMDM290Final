using UnityEngine;

public class QuitButtonScript : MonoBehaviour
{
    public float quitDelay = 1.0f;
    public AudioSource activationSound;
    public GameObject visualEffect;
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
        if (visualEffect != null)
        {
            visualEffect.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Player entered quit button trigger - quitting application");
            
            if (activationSound != null)
            {
                activationSound.Play();
            }
            
            if (visualEffect != null)
            {
                visualEffect.SetActive(true);
            }

            Invoke("QuitApplication", quitDelay);
        }
    }
    
    private void QuitApplication()
    {
        
        #if UNITY_EDITOR
    
            UnityEditor.EditorApplication.isPlaying = false;
            Debug.Log("Editor play mode stopped");
        #else
            Application.Quit();
            Debug.Log("Application has quit");
        #endif
    }
}