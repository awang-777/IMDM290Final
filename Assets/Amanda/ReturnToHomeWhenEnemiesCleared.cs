using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReturnToHomeWhenEnemiesCleared : MonoBehaviour
{
    [Header("Settings")]
    public string homeSceneName = "HomeScene"; 
    public float delayBeforeReturn = 3.0f; 
    public bool checkContinuously = true; 
    public float checkInterval = 2.0f; 
    
    [Header("Victory UI")]
    public GameObject victoryMessage; 
    private bool hasReturned = false;
    private float nextCheckTime = 0f;
    
    void Start()
    {

        if (victoryMessage != null)
        {
            victoryMessage.SetActive(false);
        }

        if (!checkContinuously)
        {
            StartCoroutine(CheckEnemiesPeriodically());
        }
    }
    
    void Update()
    {

        if (checkContinuously && !hasReturned && Time.time >= nextCheckTime)
        {
            CheckIfAllEnemiesCleared();
            nextCheckTime = Time.time + 0.5f; 
        }
    }
    
    IEnumerator CheckEnemiesPeriodically()
    {
        while (!hasReturned)
        {
            yield return new WaitForSeconds(checkInterval);
            CheckIfAllEnemiesCleared();
        }
    }
    
    void CheckIfAllEnemiesCleared()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        

        if (enemies.Length == 0)
        {
            Debug.Log("All enemies cleared!");
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

        SceneManager.LoadScene(homeSceneName);
    }
}