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
    [SerializeField] Transform nozzle;
    [SerializeField] List<Transform> enemyList;
    void Start()
    {
        currentFireTime = 0f;
    }

    void Update()
    {
        enemyList.Clear();
        foreach (Transform t in EnemyPathfinding.enemies)
        {
            if (Vector3.Distance(transform.position, t.position) <= range && !enemyList.Contains(t)) enemyList.Add(t);
            else if (Vector3.Distance(transform.position, t.position) > range && enemyList.Contains(t)) enemyList.Remove(t);
        }
        if (currentFireTime > 0) currentFireTime -= Time.deltaTime;
        if (enemyList.Count > 0)
        {
            nozzle.LookAt(ReturnFurthestEnemy());
            if (currentFireTime <= 0) SpawnProjectile();
        }
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
}
