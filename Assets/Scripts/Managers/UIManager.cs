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
    public string currentLevel = "Level 1";
    public bool gameOver = false;

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
        GameManager.Instance.OnSlotsFulled += EnableMessage;
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
        StopAllCoroutines();
        GameManager.Instance.OnSlotsFulled -= EnableMessage;
    }
    public IEnumerator AllSlotsFulled()
    {
        slotsFulledPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        slotsFulledPanel.SetActive(false);
        if(gameOver){
            yield return new WaitForSeconds(1f);
            gameOverPanel.SetActive(true);
        }
    }

    public void OnStartButtonPressed() {
        SceneManager.LoadScene(currentLevel);
    }
    public void OnLevelsButtonPressed() {
        SceneManager.LoadScene("LevelsSelection");
    }

}
