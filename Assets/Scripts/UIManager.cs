using UnityEditor.TerrainTools;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public void ReloadScene()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }
}
