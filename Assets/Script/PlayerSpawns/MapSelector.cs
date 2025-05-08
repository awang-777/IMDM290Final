using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTrigger : MonoBehaviour
{
    public int mapIndex = 1; // Set this in the inspector: 1, 2, or 3

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("SelectedMap", mapIndex);
            SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
        }
    }
}
