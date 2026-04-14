using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject gameLosePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gamePausePanel;
    private bool isSettingsPressed;
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
        gameLosePanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gamePausePanel.SetActive(false);
    }

    public void ReloadScene()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    public void EnableMessage(bool isGameWon)
    {
        StartCoroutine(AllSlotsFulled(isGameWon));
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
            GameManager.Instance.OnGameEnd -= EnableMessage;
        }
        gameWinPanel.SetActive(false);
        gameLosePanel.SetActive(false);
        gamePausePanel.SetActive(false);
        isSettingsPressed = false;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Levels" && scene.name != "StartMenu")
        {
            GameManager.Instance.OnGameEnd += EnableMessage;
        }
    }
    public IEnumerator AllSlotsFulled(bool isGameWon)
    {
        yield return new WaitForSeconds(0.5f);
        if (isGameWon)
        {
            gameWinPanel.SetActive(true);
            AudioManager.Instance.PlaySound("Win");
        }
        else if (gameOver && !isGameWon)
        {
            gameLosePanel.SetActive(true);
            AudioManager.Instance.PlaySound("Lose");
        }
    }

    public void LoadScene(string SceneName, string ButtonType)
    {
        AudioManager.Instance.PlaySound("Button");
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
        else if(ButtonType == "Default")
        {
            return;
        }
        SceneManager.LoadScene(SceneName);
    }
    public void OnSettingsButtonPressed() 
    {
        if (isSettingsPressed)
        {
            gamePausePanel.SetActive(false);
            isSettingsPressed = false;
        }
        else
        {
            gamePausePanel.SetActive(true);
            isSettingsPressed=true;
        }
    }
}
