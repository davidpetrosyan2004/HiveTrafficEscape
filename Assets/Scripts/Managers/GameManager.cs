using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static GameManager Instance { get; private set; }

    [Header("Box Path to Target References")]
    public List<Transform> hives = new();
    private Vector3[] pathPoints;
    [SerializeField] private List<Transform> parkings;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int interactableLayer;
    private readonly RayCastDetector rayCastDetector = new();

    [Header("Events")]
    public UnityAction OnSlotsFulled;

    private void Start()
    {
        pathPoints = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(pathPoints);
        InputManager.Instance.OnMouseButtonDown += AddHive;
    }

    private void Update()
    {
        MoveAllBoxes();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    public void MoveAllBoxes()
    {
        for (int i = hives.Count - 1; i >= 0; i--)
        {
            var currentHive = hives[i].GetComponentInParent<Hive>();
            if (currentHive.hasFollowed)
            {
                if (!currentHive.isFollowing)
                {
                    currentHive.isFollowing = true;
                    MoveAlongPath(hives[i].GetComponentInChildren<Collider>());
                }
            }
            else if (!currentHive.isMoving)
            {
                currentHive.isMoving = true;
                currentHive.Move();
            }
            hives.RemoveAt(i);
        }
    }

    public void AddHive()
    {
        ContactInfo contactInfo = rayCastDetector.DetectContact(interactableLayer);
        if (!hives.Contains(contactInfo.transform))
        {
            var target = GetFreeParking();
            if (contactInfo.contacted)
            {
                if (target != null)
                {
                    if(!contactInfo.transform.GetComponentInParent<Hive>().IsHiveAhead())
                    {    contactInfo.transform.GetComponentInParent<Hive>().target = target;
                        target.GetComponentInParent<Parking>().isOccupied = true;
                        hives.Add(contactInfo.transform);
                    }
                }
                else
                {
                    OnSlotsFulled?.Invoke();
                }
            }
        }
    }

    public void MoveAlongPath(Collider hive)
    {
        Vector3[] closestPath = GetClosestPath(hive.transform, pathPoints);
        hive.GetComponentInParent<Hive>().FollowPath(closestPath);
    }

    public Transform GetFreeParking()
    {
        foreach (var parking in parkings)
        {
            if (!parking.GetComponentInParent<Parking>().isOccupied)
            {
                return parking;
            }
        }
        return null;
    }

    Vector3[] GetClosestPath(Transform hive, Vector3[] points)
    {
        // 1. Choosing the closer park
        Transform targetParking = hive.GetComponentInParent<Hive>().target;
        Vector3 target = targetParking.position;
        Vector3 startPos = hive.position;


        int startIndex = GetClosestIndex(startPos, points);
        int endIndex = GetClosestIndex(target, points);

        // 2. Calculating both paths (forward and backward)
        List<Vector3> forwardPath = new();
        float forwardDist = 0;

        int i = startIndex;
        while (i != endIndex)
        {
            int next = (i + 1) % points.Length;
            forwardDist += Vector3.Distance(points[i], points[next]);
            forwardPath.Add(points[i]);
            i = next;
        }
        forwardPath.Add(points[endIndex]);

        // Backward path
        List<Vector3> backwardPath = new();
        float backwardDist = 0;

        i = startIndex;
        while (i != endIndex)
        {
            int next = (i - 1 + points.Length) % points.Length;
            backwardDist += Vector3.Distance(points[i], points[next]);
            backwardPath.Add(points[i]);
            i = next;
        }
        backwardPath.Add(points[endIndex]);

        // 3. Choosing the shorter path
        var result = forwardDist < backwardDist ? forwardPath : backwardPath;

        // 4. Adding target point to the end of the path
        result.Add(target);

        return result.ToArray();
    }

    int GetClosestIndex(Vector3 pos, Vector3[] points)
    {
        float minDist = float.MaxValue;
        int index = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float dist = Vector3.Distance(pos, points[i]);
            if (dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        return index;
    }
}
