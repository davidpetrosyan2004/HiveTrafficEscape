using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonBinder : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneName;
    [SerializeField] private TextMeshProUGUI levelText;
    public enum levelIndex
    {
        Default,
        CurrentLevel,
        Level,
        Normal,
        NextLevel,
    };
    [SerializeField] private levelIndex buttonType;
    void Start()
    {
        if(buttonType == levelIndex.Level)
        {
            levelText.text = sceneName;
        }

        button.onClick.AddListener(() => UIManager.Instance.LoadScene(sceneName, buttonType.ToString()));
    }
}
