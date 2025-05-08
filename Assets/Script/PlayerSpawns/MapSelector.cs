using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    public int mapIndex = 1; // Default map index
    private string mapSceneName => "Map" + mapIndex;
    
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
            Debug.Log("Map selector collider set to trigger mode");
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
            Debug.Log("Player entered map selector trigger - loading map scene: " + mapSceneName);
            
            if (activationSound != null)
            {
                activationSound.Play();
            }
            
            if (visualEffect != null)
            {
                visualEffect.SetActive(true);
            }
            
            Invoke("LoadMapScene", loadDelay);
        }
    }
    
    private void LoadMapScene()
    {
        if (SceneUtility.GetBuildIndexByScenePath(mapSceneName) >= 0)
        {
            SceneManager.LoadScene(mapSceneName);
        }
        else
        {
            Debug.LogError("Scene '" + mapSceneName + "' is not in the build settings! Add it in File > Build Settings");
        }
    }
    
    // Method to set the map index (can be called from UI buttons or other scripts)
    public void SetMapIndex(int index)
    {
        mapIndex = index;
        Debug.Log("Map index set to: " + mapIndex);
    }
}