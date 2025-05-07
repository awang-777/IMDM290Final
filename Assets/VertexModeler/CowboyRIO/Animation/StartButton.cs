using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public string gameSceneName = "MainScene";
    
    public float loadDelay = 1.0f;
    public AudioSource activationSound;
    public GameObject visualEffect;
    private bool hasBeenTriggered = false;
    
    private void Start()
    {
        Collider buttonCollider = GetComponent<Collider>();
        if (buttonCollider != null && !buttonCollider.isTrigger)
        {
            buttonCollider.isTrigger = true;
            Debug.Log("Start button collider set to trigger mode");
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
            Debug.Log("Player entered start button trigger - loading game scene: " + gameSceneName);
            
            if (activationSound != null)
            {
                activationSound.Play();
            }
            
            if (visualEffect != null)
            {
                visualEffect.SetActive(true);
            }
            
            Invoke("LoadGameScene", loadDelay);
        }
    }
    
    private void LoadGameScene()
    {
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