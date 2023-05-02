using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] List<Transform> path;
    [SerializeField] int pathIndex;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform currentDestination;
    void Start()
    {
        path = new();
        path.AddRange(CarvePath.Path);
    }

    void Update()
    {
        MoveTowardsCurrentPoint();
    }

    public void MoveTowardsCurrentPoint()
    {
        if (pathIndex >= path.Count) return;
        currentDestination = path[pathIndex];

        if (Vector3.Distance(transform.position, path[pathIndex].position) < 0.01f)
        {
            pathIndex++;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, path[pathIndex].position, moveSpeed * Time.deltaTime);
    }
}
