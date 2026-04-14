using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [Header("Hive Settings")]
    public int capacity = 2;
    private readonly float speed = 10f;
    public Vector3 target;
    public Parking targetParking;
    public Material roofMaterial;
    public Dictionary<Vector3, Bee> beesPos = new();
    [SerializeField] private Transform beeStartPos;
    [SerializeField] private GameObject beesParent;
    [Header("Hive References")]
    private Rigidbody rb;
    Ray ray;

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
    public bool isFulled= false;
    public bool isLastOne = false;
    public void FollowPath(Vector3[] path, bool isEnd=false)
    {
        transform.DOKill();
        OpenRoof();
        transform.DOPath(path, 2f)
            .SetLookAt(0.01f)
            .SetEase(Ease.Linear)
            .OnComplete(() => { 
                Inventory.Instance.RemoveBees();
                Parking target = GameManager.Instance.GetFreeParking();
                if (target == null && isLastOne)
                {
                    Debug.Log("Game Lose");
                    UIManager.Instance.gameOver = true; 
                    GameManager.Instance.OnGameEnd?.Invoke(false);
                }
                isLastOne = false;
            });
    }

    public void OpenRoof()
    {
        transform.GetChild(1).transform.DOLocalRotate(new Vector3(220f, 0f, 0f), 1f, RotateMode.FastBeyond360);
        beesParent.SetActive(true);
    }

    public bool IsHiveAhead()
    {
        ray = new(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
        {
            if (hitInfo.collider.CompareTag("Hive") && !hitInfo.collider.GetComponentInParent<Hive>().isMoving) {
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
    public bool IsFullWithSameColor()
    {
        int count = beesPos.Count(x => x.Value != null && x.Value.GetMaterial().name == GetMaterial().name);
        if(count >= capacity)
        {
            StartCoroutine(finish());
        }
        return count >= capacity;
    }
    
    public IEnumerator finish()
    {
        yield return new WaitForSeconds(0.6f);
        GameManager.Instance.hives.Add(this);
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
    public void AddBee(Bee bee, bool isMove = false)
    {
        BeeUtils.ParkBee(bee, beesPos, beesParent.transform, beeStartPos, isMove);
    }

    public List<Bee> RemoveBees()
    {
        return BeeUtils.RemoveBees(beesPos, roofMaterial);
    }

    private void Start()
    {
        beesParent = transform.Find("Bees").gameObject;
        beesParent.SetActive(false);
    }
}