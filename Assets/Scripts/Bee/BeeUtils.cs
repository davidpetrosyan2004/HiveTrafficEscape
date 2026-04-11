using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class BeeUtils
{
    public static void ParkBee(Bee bee, Dictionary<Vector3, Bee> positions, Transform parent, Transform reference, bool useMove = false)
    {
        // Если пчела уже где-то припаркована — сначала очистим старый слот
        if (bee.CurrentSlot != null)
        {
            var oldKey = bee.CurrentSlot.Value;
            if (positions.ContainsKey(oldKey) && positions[oldKey] == bee)
                positions[oldKey] = null;
        }

        foreach (var pos in positions.Keys.ToList())
        {
            if (positions[pos] == null)
            {
                bee.transform.parent = parent;
                bee.transform.rotation = parent.rotation;
                if (useMove)
                    bee.Move(reference.TransformPoint(pos));
                else
                    bee.transform.position = reference.TransformPoint(pos);

                positions[pos] = bee;
                bee.CurrentSlot = pos;
                break;
            }
        }
    }

    public static List<Bee> RemoveBees(Dictionary<Vector3, Bee> positions, Material roofMaterial)
    {
        List<Bee> removedBees = new();

        foreach (var key in positions.Keys.ToList())
        {
            var bee = positions[key];
            if (bee == null) continue;

            if (bee.GetMaterial().name != roofMaterial.name)
            {
                removedBees.Add(bee);
                positions[key] = null;
                bee.CurrentSlot = null;
            }
        }

        return removedBees;
    }
}