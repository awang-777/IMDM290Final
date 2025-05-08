using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public GameObject playerPrefab;

    void Start()
    {
        int mapIndex = PlayerPrefs.GetInt("SelectedMap", 1);
        Transform chosenSpawn = spawn1;

        if (mapIndex == 2) chosenSpawn = spawn2;
        else if (mapIndex == 3) chosenSpawn = spawn3;

        Instantiate(playerPrefab, chosenSpawn.position, chosenSpawn.rotation);
    }
}
