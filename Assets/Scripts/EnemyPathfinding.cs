using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public static List<Transform> enemies = new();

    [SerializeField] List<Transform> path;
    [SerializeField] int pathIndex;
    [SerializeField] float moveSpeed;
    float defaultMoveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField] Transform currentDestination;
    [SerializeField] float distanceTraveled;
    [SerializeField] float height;

    [SerializeField] float healthRemoved;

    public float DistanceTraveled => distanceTraveled;


    [SerializeField] int rank = 1;
    public int Rank => rank;
    private void Awake()
    {
        enemies.Add(transform);
        defaultMoveSpeed = moveSpeed;
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
        if (pathIndex >= path.Count)
        {
            HealthManager.health -= healthRemoved;
            HealthManager.health = Mathf.Clamp(HealthManager.health, 0, Mathf.Infinity);
            Destroy(gameObject);
        }
        if (moveSpeed > 0) MoveTowardsCurrentPoint();
        if (pathIndex < path.Count)
        {
            distanceTraveled += moveSpeed * Time.deltaTime;
        }
    }

    public void MoveTowardsCurrentPoint()
    {
        if (pathIndex >= path.Count || moveSpeed <= 0) return;
        currentDestination = path[pathIndex];
        Vector3 position = new(currentDestination.position.x, currentDestination.position.y + height, currentDestination.position.z);
        if (Vector3.Distance(transform.position, position) < 0.01f)
        {
            pathIndex++;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
    }

    public IEnumerator Slow(float percent, float duration)
    {
        print("slowing");
        moveSpeed = defaultMoveSpeed * (1 - (percent / 100));
        yield return new WaitForSeconds(duration);
        moveSpeed = defaultMoveSpeed;
    }
}
