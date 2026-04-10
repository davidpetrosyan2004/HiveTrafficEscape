using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<Material> beeMaterials = new();

    [SerializeField] private GameObject beePrefab;
    [SerializeField] private Transform Hives;
    private List<Bee> allBees = new();
    private List<Hive> allHives = new();

    private Dictionary<string, int> colorsInt = new () {
        {"Red", 0},
        {"Yellow", 1},
        {"Blue", 2}
    };
    private void Start()
    { 
        GenerateRandomBees();
        SetBeesToHives();
        CheckHivesBeesColorsSync();
    }
    public void SpawnBee(int index)
    {
        var bee = Instantiate(beePrefab);
        var beeScript = bee.GetComponent<Bee>();
        beeScript.SetMaterial(beeMaterials[index]);
        allBees.Add(beeScript);
    }
    public void GenerateRandomBees()
    {
        foreach (Hive hive in Hives.GetComponentsInChildren<Hive>())
        {
            allHives.Add(hive);
            for (int i = 0; i < hive.capacity; i++)
            {
                SpawnBee(colorsInt[hive.GetMaterial().name]);
            }
        }
        for (int i = 0; i < allBees.Count; i++)
        {
            int randomIndex = Random.Range(i, allBees.Count);
            (allBees[i], allBees[randomIndex]) = (allBees[randomIndex], allBees[i]);
        }
    }

    public void SetBeesToHives()
    {
        for (int i = 0, j = 0; i < allBees.Count && j < allHives.Count; i++)
        {
            if (!allHives[j].IsFull())
            {
                allHives[j].AddBee(allBees[i]);
            }
            else
            {
                j++;
                i--;
            }
        }
    }

    public void SwapBees(Transform bee1, Transform bee2)
    {
        (bee1.parent, bee2.parent) = (bee2.parent, bee1.parent);
        (bee1.position, bee2.position) = (bee2.position, bee1.position);
        (bee1.rotation, bee2.rotation) = (bee2.rotation, bee1.rotation);
    }

    public void CheckHivesBeesColorsSync()
    {
        foreach (var hiveObj in allHives)
        {
            var hive = hiveObj.GetComponent<Hive>();

            if (hive.HasDifferentBee()) continue;

            foreach (var otherObj in allHives)
            {
                if (otherObj == hiveObj) continue;

                var otherHive = otherObj.GetComponent<Hive>();

                var beeToSwap = otherHive.beesPos
                    .FirstOrDefault(b => b.Value.GetMaterial().name != hive.GetMaterial().name);

                if (beeToSwap.Value != null)
                {
                    var myBee = hive.beesPos.ElementAt(0).Value;

                    SwapBees(myBee.transform, beeToSwap.Value.transform);
                    return;
                }
            }
        }
    }
}
