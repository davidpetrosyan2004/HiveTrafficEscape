using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private TextMeshProUGUI levelText;
    void Start()
    {
        levelText.text = levelIndex.ToString();
    }

    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + levelIndex.ToString());
        InputManager.Instance.currentLevel = "Level " + levelIndex.ToString();
    }
}
