using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    [SerializeField] float range;
    [SerializeField] float fireRate;
    [SerializeField] float projectileSpeed;
    [SerializeField] float damage;

    private float currentFireTime;
    [SerializeField] Transform spawnPoint;
    [SerializeField, Tooltip("Only assign if the tower does not attack in a radius")] Transform nozzle;
    [SerializeField] List<Transform> enemyList;

    [SerializeField] float cost;
    [SerializeField] float costAfterBuildPhase;
    public float Cost { get { return cost; } }
    public float CostAfterBuildPhase { get { return costAfterBuildPhase; } }


    [SerializeField] bool stop;
    [SerializeField] bool targetEnemiesOrAttackInRadius;
    private void Awake()
    {
        HealthManager.onDeath += OnDeath;
    }
    void Start()
    {
        currentFireTime = 0f;
    }

    void Update()
    {
        if (stop) return;
        enemyList.Clear();
        if (currentFireTime > 0) currentFireTime -= Time.deltaTime;

        if (targetEnemiesOrAttackInRadius) TargetedShoot();
        else ShootInRadius();
    }

    public Transform ReturnFurthestEnemy()
    {
        float maxDistance = enemyList[0].GetComponent<EnemyPathfinding>().DistanceTraveled;
        Transform furthest = enemyList[0];
        for (int i = 1; i < enemyList.Count; i++)
        {
            Transform currentTransform = enemyList[i];
            float distance = currentTransform.GetComponent<EnemyPathfinding>().DistanceTraveled;
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthest = currentTransform;
            }
        }
        return furthest;
    }

    public void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        TowerProjectile tp = clone.GetComponent<TowerProjectile>();
        rb.velocity = spawnPoint.forward * projectileSpeed;
        tp.SetDamage(damage);
        currentFireTime = fireRate;
    }

    public void OnDeath() => stop = true;

    public void TargetedShoot()
    {
        foreach (Transform t in EnemyPathfinding.enemies)
        {
            if (Vector3.Distance(transform.position, t.position) <= range && !enemyList.Contains(t)) enemyList.Add(t);
            else if (Vector3.Distance(transform.position, t.position) > range && enemyList.Contains(t)) enemyList.Remove(t);
        }
        if (enemyList.Count > 0)
        {
            nozzle.LookAt(ReturnFurthestEnemy());
            if (currentFireTime <= 0) SpawnProjectile();
        }
    }

    public void ShootInRadius()
    {

    }
}
