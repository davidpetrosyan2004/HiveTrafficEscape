using UnityEngine;

public class Parking : MonoBehaviour
{
    public bool isOccupied = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hive"))
        {
            var hiveScript = other.GetComponentInParent<Hive>();
            var bees = hiveScript.PushBees();
            Inventory.Instance.AddBees(bees);
            Inventory.Instance.AddHive(hiveScript);
            //Inventory.Instance.RemoveBees();
        }
    }
}
