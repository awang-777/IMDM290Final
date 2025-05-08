using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTrigger : MonoBehaviour
{
    public int mapIndex = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("SelectedMap", mapIndex);
            SceneManager.LoadScene("GameScene");
        }
    }
}
