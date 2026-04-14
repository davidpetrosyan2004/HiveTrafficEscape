using TMPro;
using UnityEngine;

public class House : MonoBehaviour
{
    private static int allHiveCount;
    [SerializeField] private int inspectorCount;
    [SerializeField] private int hivesCount;
    [SerializeField] private int hivesRemained;
    [SerializeField] private TextMeshProUGUI hivesCountText;
    [SerializeField] private Animator animator;
    void Start()
    {
        allHiveCount = inspectorCount;
        hivesCountText.text = "0/" + hivesRemained.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        var root = other.transform.parent;

        if (!root.CompareTag("Hive")) return;
        allHiveCount--;
        hivesCount++;
        hivesCountText.text = hivesCount.ToString() + "/" + hivesRemained.ToString();
        animator.SetTrigger("Scale");
        Destroy(other.transform.parent.gameObject);
        AudioManager.Instance.PlaySound("HouseEnter");
        if(allHiveCount <= 0)
        {
            GameManager.Instance.OnGameEnd?.Invoke(true);
        }
    }
}
