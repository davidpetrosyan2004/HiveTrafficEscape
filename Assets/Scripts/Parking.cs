using UnityEngine;

public class Parking : MonoBehaviour
{
    public bool isOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hive"))
        {
            other.GetComponentInParent<Hive>().OpenRoof();
        }
    }
}
