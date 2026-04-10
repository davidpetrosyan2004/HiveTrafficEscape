using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Inventory : MonoBehaviour
{
    private static Dictionary<Vector3, Bee> slotsPos = new();
    [SerializeField] private int capacity;
    [SerializeField] private Transform slotStartPos;
    [SerializeField] private Transform beePrefab;
    //[SerializeField] private Transform slotPrefab;
    public static Inventory Instance { get; set; }

    private void Awake()
    {
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
                if (!slotsPos[slotPos])
                {
                    bee.Move(slotPos);
                    slotsPos[slotPos] = bee;
                    bee.transform.parent = transform;
                    break;
                }
            }
        }
    }
}
