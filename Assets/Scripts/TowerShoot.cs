using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TowerShoot : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    [SerializeField] float range;
    [SerializeField] float fireRate;
    [SerializeField] float projectileSpeed;
    [SerializeField] float damage;

    private float currentFireTime;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int attackAmount;
    [SerializeField] float timeBetweenAttacks;

    [SerializeField, Tooltip("Only assign if the tower does not attack in a radius")] Transform nozzle;
    [SerializeField] List<Transform> enemyList;
    public List<Transform> EnemyList { get { return enemyList; } }

    [Header("Order: Wood, Stone, Iron, Gold (add more later)")]
    [SerializeField] List<float> resourceCosts;
    public List<float> ResourceCosts { get { return resourceCosts; } }
    [SerializeField] List<float> resourceCostsAfterBuildPhase;
    public List<float> ResourceCostsAfterBuildPhase { get { return resourceCostsAfterBuildPhase; } }

    [SerializeField] bool stop;
    [SerializeField] bool targetEnemiesOrAttackInRadius;

    [SerializeField] float attackDelay;
    bool waiting;
    bool attackWaiting;
    public event Action OnShoot;
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
        if (stop || SpawnTower.instance.BuildPhase) return;
        enemyList.Clear();
        foreach (Transform t in EnemyPathfinding.enemies)
        {
            if (Vector3.Distance(transform.position, t.position) <= range && !enemyList.Contains(t)) enemyList.Add(t);
            else if (Vector3.Distance(transform.position, t.position) > range && enemyList.Contains(t)) enemyList.Remove(t);
        }

        if (currentFireTime > 0) currentFireTime -= Time.deltaTime;
        if (currentFireTime <= 0 && enemyList.Count > 0)
        {
            if (waiting || attackWaiting) return;
            StartCoroutine(WaitAndShoot(attackDelay));
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
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            TowerProjectile tp = clone.GetComponent<TowerProjectile>();
            rb.velocity = spawnPoint.forward * projectileSpeed;
            tp.SetDamage(damage);
        }

        currentFireTime = fireRate;
    }

    public IEnumerator SpawnProjectileDelayed()
    {
        attackWaiting = true;
        for (int i = 0; i < attackAmount; i++)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                GameObject clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
                Rigidbody rb = clone.GetComponent<Rigidbody>();
                TowerProjectile tp = clone.GetComponent<TowerProjectile>();
                rb.velocity = spawnPoint.forward * projectileSpeed;
                tp.SetDamage(damage);
            }
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
        attackWaiting = false;
        currentFireTime = fireRate;
    }

    public void OnDeath() => stop = true;

    public void TargetedShoot()
    {
        if (enemyList.Count > 0)
        {
            nozzle.LookAt(ReturnFurthestEnemy());
            if (currentFireTime <= 0 && !attackWaiting)
            {
                if (attackAmount <= 1) SpawnProjectile();
                else StartCoroutine(SpawnProjectileDelayed());
            }
        }
    }

    public void ShootInRadius()
    {
        if (enemyList.Count > 0 && currentFireTime <= 0)
        {
            foreach (Transform t in enemyList) t.GetComponent<EnemyHealth>().DecreaseHealth(damage);
            currentFireTime = fireRate;
        }
    }

    public IEnumerator Wait(float t)
    {
        attackWaiting = true;
        yield return new WaitForSeconds(t);
        attackWaiting = false;
    }

    public IEnumerator WaitAndShoot(float t)
    {
        OnShoot?.Invoke();
        waiting = true;
        yield return new WaitForSeconds(t);
        if (targetEnemiesOrAttackInRadius) TargetedShoot();
        else ShootInRadius();
        waiting = false;
    }
}
