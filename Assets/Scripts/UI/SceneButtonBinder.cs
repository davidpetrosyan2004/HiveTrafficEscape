using UnityEngine;
using UnityEngine.UI;

public class SceneButtonBinder : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneName;
    public enum levelIndex
    {
        Start,
        Level,
        Normal
    };
    [SerializeField] private levelIndex buttonType;
    void Start()
    {
        button.onClick.AddListener(() => UIManager.Instance.LoadScene(sceneName, buttonType.ToString()));
    }
}
