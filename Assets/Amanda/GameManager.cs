using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("UI References")]
    public TextMeshProUGUI elimsText;
    
    [Header("Game Stats")]
    public int elimCount = 0;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        UpdateElimsUI();
    }
    
    public void AddElimination()
    {
        elimCount++;
        UpdateElimsUI();
        Debug.Log("Enemy eliminated! Total: " + elimCount);
    }
    
    private void UpdateElimsUI()
    {
        if (elimsText != null)
        {
            elimsText.text = "Eliminations: " + elimCount;
        }
    }
}