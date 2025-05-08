using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    public string targetSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("SelectedMap", targetSceneName);
            PlayerPrefs.Save();
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
