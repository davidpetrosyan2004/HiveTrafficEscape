using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.MaterialProperty;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;


public class Hive : MonoBehaviour
{
    [Header("Hive Settings")]
    [SerializeField] private Color[] colors;
    [SerializeField] private Bee beePrefab;
    private readonly float speed = 10f;
    public Transform target;

    [Header("Hive Components")]
    private Rigidbody rb;


    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        //SetColor();
    }

    //public void SetColor()
    //{
    //    Color hiveColor = GetRandomColor();

    //    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = hiveColor;
    //    transform.GetChild(1).GetComponent<MeshRenderer>().material.color = hiveColor;
    //}

    //private Color GetRandomColor()
    //{
    //    return colors[Random.Range(0, colors.Length)];
    //}

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
        transform.GetChild(1).DORotate(new Vector3(0f, 0f, -90f), 1f)
            .SetEase(Ease.OutCirc);
    }

    public bool IsHiveAhead()
    {
        Ray ray = new(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
        {
            //Debug.Log(hitInfo.collider.name);
            if (hitInfo.collider.CompareTag("Hive")) { 
                Debug.Log("Hive ahead!");
                return true; }
        }
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 300f, Color.red, 1f);

        return false;
    }
}