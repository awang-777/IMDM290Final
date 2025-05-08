using UnityEngine;

public class MapSpawnManager : MonoBehaviour
{
    public GameObject playerPrefab; 
    public Transform spawnPoint; 
    private void Start()
    {
       
        if (spawnPoint == null)
            spawnPoint = this.transform;

        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");

        if (existingPlayer != null)
        {
            existingPlayer.transform.position = spawnPoint.position;
            existingPlayer.transform.rotation = spawnPoint.rotation;
        }
        else if (playerPrefab != null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            newPlayer.tag = "Player";
        }
        else
        {
            Debug.LogError("No player prefab assigned and no player found in scene.");
        }
    }
}
