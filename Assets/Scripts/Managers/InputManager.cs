using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public UnityAction OnMouseButtonDown;
    private string levelSelectorScene = "LevelsSelector";
    public string currentLevel;
    private bool isLoading = false;

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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.P) && !isLoading)
        {
            string activeScene = SceneManager.GetActiveScene().name;

            if (activeScene == levelSelectorScene)
            {
                LoadSceneSafe(currentLevel);
            }
            else
            {
                LoadSceneSafe(levelSelectorScene);
            }
            Debug.Log("P pressed");
        }
    }

    void LoadSceneSafe(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName) return;

        isLoading = true;
        SceneManager.LoadScene(sceneName);
    }
}
