using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReturnToHomeWhenEnemiesCleared : MonoBehaviour
{
    [Header("Settings")]
    public string homeSceneName = "HomeScene"; 
    public float delayBeforeReturn = 3.0f;
    public float checkInterval = 2.0f;
    
    [Header("Victory UI")]
    public GameObject victoryMessage;
    
    private bool hasReturned = false;
    private int initialEnemyCount = 0;
    private float timeSinceLastCheck = 0f;
    
    void Start()
    {
        if (victoryMessage != null)
        {
            victoryMessage.SetActive(false);
        }
        
        initialEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"Initial enemy count: {initialEnemyCount}");
        
        StartCoroutine(DelayedStart());
    }
    
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        
        StartCoroutine(CheckEnemiesPeriodically());
    }
    
    IEnumerator CheckEnemiesPeriodically()
    {
        while (!hasReturned)
        {
            yield return new WaitForSeconds(checkInterval);
            
            if (!hasReturned)
            {
                CheckIfAllEnemiesCleared();
            }
        }
    }
    
    void CheckIfAllEnemiesCleared()
    {
        GameObject[] taggedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        int enemiesWithController = 0;
        foreach (GameObject enemy in taggedEnemies)
        {
            if (enemy.GetComponent<EnemyController>() != null)
            {
                enemiesWithController++;
            }
        }
        
        Debug.Log($"Current enemy count: {enemiesWithController}");
        
        if (initialEnemyCount > 0 && enemiesWithController == 0)
        {
            Debug.Log("All enemies have been cleared!");
            hasReturned = true;
            StartCoroutine(ReturnToHomeAfterDelay());
        }
    }
    
    IEnumerator ReturnToHomeAfterDelay()
    {
        if (victoryMessage != null)
        {
            victoryMessage.SetActive(true);
        }
        
        yield return new WaitForSeconds(delayBeforeReturn);
        
        if (SceneUtility.GetBuildIndexByScenePath(homeSceneName) >= 0)
        {
            SceneManager.LoadScene(homeSceneName);
        }
        else
        {
            Debug.LogError($"Scene '{homeSceneName}' does not exist in build settings. Cannot load.");
        }
    }
}