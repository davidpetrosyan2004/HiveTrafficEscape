using UnityEngine;

public class Parking : MonoBehaviour
{
    public bool isOccupied = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hive"))
            return;

        var hiveScript = other.GetComponentInParent<Hive>();

        var bees = hiveScript.RemoveBees();

        Inventory.Instance.AddHive(hiveScript);
        Inventory.Instance.AddBees(bees);
    }
}
