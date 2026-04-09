using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private TextMeshProUGUI levelText;

    //private void Start()
    //{
    //    levelText.text = level.ToString();
    //}

    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + level.ToString());
    }

}
