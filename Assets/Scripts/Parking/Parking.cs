using UnityEngine;

public class Parking : MonoBehaviour
{
    public bool isOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hive"))
        {
            var hiveScript = other.GetComponentInParent<Hive>();
            //hiveScript.OpenRoof();
            var bees = hiveScript.PushBees();
            Inventory.Instance.AddBees(bees);
        }
    }
}
