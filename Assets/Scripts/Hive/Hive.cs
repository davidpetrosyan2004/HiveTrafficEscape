using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Hive : MonoBehaviour
{
    [Header("Hive Settings")]
    public int capacity = 2;
    private readonly float speed = 10f;
    public Parking target = null;
    public Material roofMaterial;
    public Dictionary<Vector3, Bee> beesPos = new();
    [SerializeField] private Transform beeStartPos;
    [SerializeField] private Transform beePrefab;

    [Header("Hive References")]
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

            //Vector3 worldPos = beeStartPos.TransformPoint(localOffset);

            beesPos.Add(localOffset, null);
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
        return false;
    }

    public bool IsFull()
    {
        int count = beesPos.Count(x => x.Value != null);
        return count >= capacity;
    }
    
    public void AddBee(Bee bee, bool isMove=false)
    {
        bee.transform.parent = transform;
        foreach (var pos in beesPos)
        {
            if (pos.Value == null)
            {
                if(isMove)
                    bee.Move(beeStartPos.TransformPoint(pos.Key));
                else
                    bee.transform.position = beeStartPos.TransformPoint(pos.Key);
                bee.transform.rotation = transform.rotation;
                beesPos[pos.Key] = bee;
                break;
            }
        }
    }

    public bool HasDifferentBee()
    {
        var myMaterial = GetMaterial().name;

        foreach (var b in beesPos)
        {
            if (b.Value == null) continue;

            if (b.Value.GetMaterial().name != myMaterial)
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
                newBees.Add(beesPos[beePos]);
                beesPos[beePos] = null;
            }
        }
        return newBees;
    }

}