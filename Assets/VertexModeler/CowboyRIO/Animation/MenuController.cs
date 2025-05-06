using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class MenuController : MonoBehaviour
{
    public string gameplaySceneName; // Set this in the Inspector to your gameplay scene name
    
    [Header("Development Options")]
    [SerializeField] private bool skipStartScreen = false; // Toggle this in the inspector for testing
    
    private void Start()
    {
        // If skipStartScreen is enabled, immediately load the gameplay scene
        if (skipStartScreen)
        {
            Debug.Log("Skip start screen enabled - loading gameplay scene directly");
            SceneManager.LoadScene(gameplaySceneName);
        }
    }
    
    // Call this function from the Start button's onClick event
    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
    
    // Call this function from the Quit button's onClick event
    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // This message will appear in the console
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the editor
        #else
        Application.Quit(); // Quits the application when built
        #endif
    }
}