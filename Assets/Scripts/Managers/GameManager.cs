using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static GameManager Instance { get; private set; }

    [Header("Box Path to Target References")]
    public List<Hive> hives = new();
    private Vector3[] parkingPathPoints;
    private Vector3[] finishPathPoints;
    [SerializeField] private List<Parking> parkings;
    [SerializeField] private List<Transform> finishPaths;

    [Header("References")]
    [SerializeField] private LineRenderer parkingPathLine;
    [SerializeField] private LineRenderer finishPathLine;
    [SerializeField] private int interactableLayer;
    private readonly RayCastDetector rayCastDetector = new();

    [Header("Events")]
    public UnityAction<bool> OnGameEnd;

    private void Start()
    {
        parkingPathPoints = new Vector3[parkingPathLine.positionCount];
        parkingPathLine.GetPositions(parkingPathPoints);

        finishPathPoints = new Vector3[parkingPathLine.positionCount];
        finishPathLine.GetPositions(finishPathPoints);

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
            var currentHive = hives[i];
            if(currentHive.isFulled)
            {
                if(currentHive.GetMaterial().name == "Red")
                {
                    currentHive.target = finishPaths[0].position;
                }
                else if (currentHive.GetMaterial().name == "Blue")
                {
                    currentHive.target = finishPaths[1].position;
                }
                else if (currentHive.GetMaterial().name == "Yellow")
                {
                    currentHive.target = finishPaths[2].position;
                }
                MoveAlongPath(hives[i], finishPathPoints, true);
            }
            else if (currentHive.hasFollowed)
            {
                if (!currentHive.isFollowing)
                {
                    currentHive.isFollowing = true;
                    MoveAlongPath(hives[i], parkingPathPoints);
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
        if (!hives.Contains(contactInfo.hive) && contactInfo.hive != null)
        {
            if (contactInfo.contacted)
            {
                Parking target = GetFreeParking();
                if (target != null)
                {
                    if (!contactInfo.hive.IsHiveAhead())
                    {
                        target.isOccupied = true;
                        Parking targetParking = GetFreeParking();
                        if (targetParking == null)
                        {
                            Debug.Log("Last");
                            contactInfo.hive.isLastOne = true;
                        }
                        contactInfo.hive.target = target.transform.position;
                        contactInfo.hive.targetParking = target;
                        hives.Add(contactInfo.hive);
                    }  
                }
            }
        }
    }

    public void MoveAlongPath(Hive hive, Vector3[] pathPoints, bool isEnd=false)
    {
        Vector3[] closestPath = GetClosestPath(hive, pathPoints);
        hive.FollowPath(closestPath, isEnd);
    }

    public Parking GetFreeParking()
    {
        foreach (Parking parking in parkings)
        {
            if (!parking.isOccupied)
            {
                return parking;
            }
        }
        return null;
    }

    Vector3[] GetClosestPath(Hive hive, Vector3[] points)
    {
        // 1. Choosing the closer park
        //Vector3 targetParking = hive.target;
        Vector3 target = hive.target;
        Vector3 startPos = hive.transform.position;


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
