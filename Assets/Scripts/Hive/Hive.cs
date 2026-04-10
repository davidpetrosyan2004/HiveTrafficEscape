using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [Header("Hive Settings")]
    public int capacity = 2;
    private readonly float speed = 10f;
    public Transform target = null;
    //public List<Bee> bees = new();
    public Material roofMaterial;
    public Dictionary<Vector3, Bee> beesPos = new();
    [SerializeField] private Transform beeStartPos;

    [Header("Hive References")]
    [SerializeField] private Bee beePrefab;
    private Rigidbody rb;
     

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();

        var roof = transform.Find("Roof");
        roofMaterial = roof.GetComponent<MeshRenderer>().sharedMaterial;

        for (int i = 0; i < capacity; i++)
        {
            Vector3 localOffset;

            if (i < capacity / 2)
                localOffset = new Vector3(0, 0, 0.5f * i);
            else
                localOffset = new Vector3(0.5f, 0, 0.5f * (i - capacity / 2));

            Vector3 worldPos = beeStartPos.TransformPoint(localOffset);

            beesPos.Add(worldPos, null);
        }

    }

    public Material GetMaterial()
    {
        return roofMaterial;
    }
    public void SetMaterial(Material newMaterial)
    {
        roofMaterial = newMaterial;
        transform.Find("Body").GetComponent<MeshRenderer>().sharedMaterial = roofMaterial;
    }

    public void Move()
    {
        transform.DOBlendableMoveBy( transform.forward, 1f / speed)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Incremental);
    }
    public bool isMoving = false;
    public bool hasFollowed = false;
    public bool isFollowing = false;
    public void FollowPath(Vector3[] path)
    {
        transform.DOKill();
        OpenRoof();
        rb.DOPath(path, 2f)
            .SetLookAt(0.01f)
            .SetEase(Ease.Linear);
    }

    public void OpenRoof()
    {
        transform.GetChild(1).transform.DOLocalRotate(new Vector3(220f, 0f, 0f), 1f, RotateMode.FastBeyond360);

    }

    public bool IsHiveAhead()
    {
        Ray ray = new(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
        {
            if (hitInfo.collider.CompareTag("Hive")) {
                transform.DOMove(hitInfo.point + (transform.position-hitInfo.point).normalized * 0.5f + new Vector3(0, -0.2f, 0), 0.1f)
                         .SetLoops(2, LoopType.Yoyo);
                return true; }
        }
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 300f, Color.red, 1f);

        return false;
    }

    public bool IsFull()
    {
        int count = beesPos.Count(x => x.Value != null);
        return count >= capacity;
    }
    
    public void AddBee(Bee bee)
    {
        //bees.Add(bee);
        bee.transform.parent = transform;
        foreach (var pos in beesPos)
        {
            if (!pos.Value)
            {
                bee.transform.position = pos.Key;
                bee.transform.rotation = transform.rotation;
                beesPos[pos.Key] = bee;
                break;
            }
        }
    }

    public bool HasDifferentBee()
    {
        foreach (var b in beesPos.Values)
        {
            if (b.GetMaterial().name != roofMaterial.name)
            {
                return true;
            }
        }
        return false;
    }

    public List<Bee> PushBees()
    {
        List<Bee> newBees = new List<Bee>();
        for (int i = beesPos.Values.Count - 1; i >= 0; i--)
        {
            var beePos = beesPos.ElementAt(i).Key;
            if (beesPos[beePos].GetMaterial().name != roofMaterial.name)
            {
                Debug.Log(beesPos[beePos].GetMaterial().name);
                newBees.Add(beesPos[beePos]);
                beesPos[beePos] = null;
            }
        }
        return newBees;
    }
    public void PullBees()
    {

    }
}