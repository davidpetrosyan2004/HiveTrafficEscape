using UnityEngine;
using UnityEngine.UIElements;

public class Border : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hive"))
        {
            other.GetComponentInParent<Hive>().hasFollowed = true;
            GameManager.Instance.hives.Add(other.GetComponentInParent<Hive>());
        }
    }
}
