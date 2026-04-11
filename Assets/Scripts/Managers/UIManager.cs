using UnityEditor.TerrainTools;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
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
    private void OnDestroy()
    {
        StopAllCoroutines();
        GameManager.Instance.OnSlotsFulled -= EnableMessage;
    }
    public IEnumerator AllSlotsFulled()
    {
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(false);
    }
}
