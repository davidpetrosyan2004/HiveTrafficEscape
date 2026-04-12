using UnityEditor.TerrainTools;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject gameEndPanel;
    public bool gameOver = false;
    public int currentLevel = 1;

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
        gameEndPanel.SetActive(false);
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
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name != "Levels" && scene.name != "StartMenu")
        {
            GameManager.Instance.OnSlotsFulled -= EnableMessage;
        }
        gameEndPanel.SetActive(false);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Levels" && scene.name != "StartMenu")
        {
            GameManager.Instance.OnSlotsFulled += EnableMessage;
            Debug.Log("Suscribe");
        }
    }
    public IEnumerator AllSlotsFulled()
    {
        if(gameOver){
            yield return new WaitForSeconds(0.5f);
            gameEndPanel.SetActive(true);
        }
    }

    public void LoadScene(string SceneName, string ButtonType)
    {
        if (ButtonType == "CurrentLevel")
        {
            SceneName = "Level " + currentLevel.ToString();
        }
        else if(ButtonType == "Level")
        {
            SceneName = "Level " + SceneName;
        }
        else if(ButtonType == "NextLevel")
        {
            currentLevel++;
            SceneName = "Level " + currentLevel.ToString();
        }
        AudioManager.Instance.PlaySound("Button");
        SceneManager.LoadScene(SceneName);
    }

}
