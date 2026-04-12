using UnityEditor.TerrainTools;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject slotsFulledPanel;
    [SerializeField] private GameObject gameOverPanel;
    public bool gameOver = false;
    public string currentLevel = "1";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

    }
    private void Start()
    {
        gameOverPanel.SetActive(false);
        slotsFulledPanel.SetActive(false);
    }

    public void ReloadScene()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    public void EnableMessage()
    {
        StartCoroutine(AllSlotsFulled());
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        gameOver = false;
        if (SceneManager.GetActiveScene().name != "Levels" && SceneManager.GetActiveScene().name != "StartMenu")
            GameManager.Instance.OnSlotsFulled -= EnableMessage;
    }

    private void OnEnable()
    {
        if(SceneManager.GetActiveScene().name != "Levels" && SceneManager.GetActiveScene().name != "StartMenu")
            GameManager.Instance.OnSlotsFulled += EnableMessage;
    }
    public IEnumerator AllSlotsFulled()
    {
        slotsFulledPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        slotsFulledPanel.SetActive(false);
        if(gameOver){
            yield return new WaitForSeconds(0.8f);
            gameOverPanel.SetActive(true);
        }
    }

    public void LoadScene(string SceneName, string ButtonType)
    {
        if (ButtonType == "Start")
        {
            SceneName = "Level " + currentLevel;
        }
        else if(ButtonType == "Level")
        {
            SceneName = "Level " + SceneName;
        }
        AudioManager.Instance.PlaySound("Button");
        SceneManager.LoadScene(SceneName);
    }

}
