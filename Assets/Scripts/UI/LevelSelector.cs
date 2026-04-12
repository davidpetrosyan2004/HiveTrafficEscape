using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public enum levelIndex { 
        Start,
        Back,
        Level
    };
    [SerializeField] private levelIndex buttonType;
    [SerializeField] private TextMeshProUGUI levelText;
    //void Start()
    //{
    //    levelText.text = levelIndex.ToString();
    //}

    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + buttonType.ToString());
        //InputManager.Instance.currentLevel = "Level " + levelIndex.ToString();
    }
}
