using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public static List<Transform> enemies = new();

    [SerializeField] List<Transform> path;
    [SerializeField] int pathIndex;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform currentDestination;
    [SerializeField] float distanceTraveled;
    [SerializeField] float height;
    public float DistanceTraveled { get { return distanceTraveled; } }
    private void Awake()
    {
        enemies.Add(transform);    
    }

    private void OnDestroy()
    {
        enemies.Remove(transform);
    }

    void Start()
    {
        path = new();
        path.AddRange(CarvePath.Path);
    }

    void Update()
    {
        MoveTowardsCurrentPoint();
        if (pathIndex < path.Count)
        {
            distanceTraveled += moveSpeed * Time.deltaTime;
        }
    }

    public void MoveTowardsCurrentPoint()
    {
        if (pathIndex >= path.Count) return;
        currentDestination = path[pathIndex];
        Vector3 position = new(currentDestination.position.x, currentDestination.position.y + height, currentDestination.position.z);
        if (Vector3.Distance(transform.position, position) < 0.01f)
        {
            pathIndex++;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
    }
}
