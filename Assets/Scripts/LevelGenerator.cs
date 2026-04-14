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
        {"Blue", 1},
        {"Yellow", 2},
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

    public void SwapBees(Bee bee1, Bee bee2)
    {
        (bee1.transform.parent, bee2.transform.parent) = (bee2.transform.parent, bee1.transform.parent);
        (bee1.transform.position, bee2.transform.position) = (bee2.transform.position, bee1.transform.position);
        (bee1.transform.rotation, bee2.transform.rotation) = (bee2.transform.rotation, bee1.transform.rotation);
    }

    public void CheckHivesBeesColorsSync()
    {
        foreach (var hive in allHives)
        {
            if (hive.HasDifferentBee()) continue;

            foreach (var otherHive in allHives)
            {
                if (otherHive.GetMaterial().name == hive.GetMaterial().name) continue;

                var beeToSwap = otherHive.beesPos
                    .FirstOrDefault(b => b.Value.GetMaterial().name != hive.GetMaterial().name);

                if (beeToSwap.Value != null)
                {
                    var myBee = hive.beesPos.ElementAt(0);

                    var bee1 = myBee.Value;
                    var bee2 = beeToSwap.Value;

                    SwapBees(bee1, bee2);

                    hive.beesPos[myBee.Key] = bee2;
                    otherHive.beesPos[beeToSwap.Key] = bee1;
                    //Debug.Log("1 " + hive.name + "2 " + otherHive.name);
                    return;
                }
            }
        }
    }
}
