using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity;

    [Header("Lists")]
    private static Dictionary<Vector3, Bee> slotsPos = new();
    private List<Hive> hives = new();

    [Header("References Settings")]
    [SerializeField] private Transform slotStartPos;
    [SerializeField] private Transform slotPrefab;
    public static Inventory Instance { get; set; }

    private void Awake()
    {
        slotsPos.Clear();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    void Start()
    {
        SetSlotsPos();
    }
    private void SetSlotsPos()
    {
        for (int i = 0; i < capacity; i++)
        {
            Vector3 localOffset;

            if (i < capacity / 2)
                localOffset = new Vector3(1f * i, 0, 0);
            else
                localOffset = new Vector3(1f * (i - capacity / 2), 0, 1f);

            Vector3 worldPos = slotStartPos.TransformPoint(localOffset);
            Instantiate(slotPrefab, worldPos+ new Vector3(0, 0.1f, 0), slotPrefab.transform.rotation);
            slotsPos.Add(worldPos, null);
        }
    }
    public void AddBees(List<Bee> bees)
    {

        foreach (Bee bee in bees)
        {
            for (int i = 0; i < slotsPos.Count; i++)
            {
                var slotPos = slotsPos.ElementAt(i).Key;
                if (slotsPos[slotPos] == null)
                {
                    bee.Move(slotPos);
                    slotsPos[slotPos] = bee;
                    bee.transform.parent = transform;
                    break;
                }
            }
        }
    }

    public void RemoveBees()
    {
        var keys = slotsPos.Keys.ToList();
        foreach (var key in keys)
        {
            var value = slotsPos[key];
            if (value == null) continue;

            var hive = GetHiveTheSameColor(value);
            if (hive == null) continue;

            hive.AddBee(value, true);
            slotsPos[key] = null;
        }
    }

    public Hive GetHiveTheSameColor(Bee bee)
    {
        foreach (Hive hive in hives)
        {
            Debug.Log(hive.name);
            if(bee.GetMaterial().name == hive.GetMaterial().name)
            {
                return hive;
            }
        }
        return null;
    }
    public void AddHive(Hive hive)
    {
        hives.Add(hive);
    }
}
