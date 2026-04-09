using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class Hive : MonoBehaviour
{
    [Header("Hive Settings")]
    public int capacity = 2;
    private readonly float speed = 10f;
    public Transform target = null;
    public List<Bee> bees = new();
    public Material roofMaterial;
    private Dictionary<Vector3, bool> beesPos = new();
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

            beesPos.Add(worldPos, false);
        }

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
        rb.DOPath(path, 2f)
            .SetLookAt(0.01f)
            .SetEase(Ease.Linear);
    }

    public void OpenRoof()
    {
        transform.GetChild(1).DORotate(new Vector3(0f, 0f, 90f), 1f)
            .SetEase(Ease.OutCirc);
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
        return bees.Count >= capacity;
    }
    
    public void AddBee(Bee bee)
    {
        bees.Add(bee);
        bee.transform.SetParent(transform);
        foreach (var pos in beesPos)
        {
            if (!pos.Value)
            {
                bee.transform.position = pos.Key;
                bee.transform.rotation = transform.rotation;
                beesPos[pos.Key] = true;
                break;
            }
        }
    }

    public bool HasDifferentBee()
    {
        foreach (var b in bees)
        {
            if (b.GetMaterial().name != roofMaterial.name)
            {
                return true;
            }
        }
        return false;
    }

    public void PushBees()
    {
         
    }
    public void PullBees()
    {

    }
}