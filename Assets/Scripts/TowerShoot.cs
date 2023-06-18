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
    public void SetDamage(float d) => damage = d;
    private float currentFireTime;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int attackAmount;
    [SerializeField] float timeBetweenAttacks;

    [SerializeField, Tooltip("Only assign if the tower does not attack in a radius")] Transform nozzle;
    [SerializeField] List<Transform> enemyList;
    public List<Transform> EnemyList { get { return enemyList; } }
    
    [SerializeField] Transform targetedEnemy;
    public Transform TargetedEnemy { get { return targetedEnemy; } }

    [Header("Order: Wood, Stone, Iron, Gold (add more later)")]
    [SerializeField] List<float> resourceCosts;
    public List<float> ResourceCosts { get { return resourceCosts; } }
    [SerializeField] List<float> resourceCostsAfterBuildPhase;
    public List<float> ResourceCostsAfterBuildPhase { get { return resourceCostsAfterBuildPhase; } }

    [SerializeField] bool stop;
    [SerializeField] bool targetEnemiesOrAttackInRadius = true;

    [SerializeField] float attackDelay;
    bool waiting;
    bool attackWaiting;
    public event Action OnShoot;

    public TowerElement element;
    private void Awake()
    {
        HealthManager.onDeath += OnDeath;
        element = GetComponent<TowerElement>();
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
        if (enemyList.Count == 0) return null;
        //float maxDistance = enemyList[0].GetComponent<EnemyPathfinding>().DistanceTraveled;
        float maxDistance = -1;
        Transform furthest = null;
        //Transform furthest = enemyList[0];
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null) continue;
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
            tp.parent = this;
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
            Vector3 targetedEnemy = ReturnFurthestEnemy().position;
            if (nozzle) nozzle.LookAt(new Vector3(targetedEnemy.x, transform.position.y, targetedEnemy.z));
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
