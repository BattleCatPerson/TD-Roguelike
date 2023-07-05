using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Targeting
{
    First = 1,
    Last = 2,
    Strong = 3,
    Close = 4
}
public class TowerShoot : MonoBehaviour
{

    [SerializeField] GameObject projectile;
    [SerializeField] float range;
    public float Range => range;
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
    public List<Transform> EnemyList => enemyList;

    [SerializeField] Transform targetedEnemy;
    public Transform potentialEnemy;
    public Transform TargetedEnemy => targetedEnemy;

    [Header("Order: Wood, Stone, Iron, Gold (add more later)")]
    [SerializeField] List<float> resourceCosts;
    public List<float> ResourceCosts => resourceCosts;
    [SerializeField] List<float> resourceCostsAfterBuildPhase;
    public List<float> ResourceCostsAfterBuildPhase => resourceCostsAfterBuildPhase;

    [SerializeField] bool stop;
    [SerializeField] bool targetEnemiesOrAttackInRadius = true;
    public bool TargetEnemiesOrAttackInRadius => targetEnemiesOrAttackInRadius;

    [SerializeField] float attackDelay;
    bool waiting;
    bool attackWaiting;
    public event Action OnShoot;

    public TowerElement element;

    [SerializeField] float totalDamage;
    public float TotalDamage { get { return totalDamage; } set { totalDamage = value; } }

    public Targeting target = Targeting.First;
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
        if (GetComponent<TowerProjectile>()) target = GetComponent<TowerProjectile>().parent.target;

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

        if (enemyList.Count > 0)
        {
            if (target == Targeting.First) targetedEnemy = ReturnFurthestEnemy();
            else if (target == Targeting.Last) targetedEnemy = ReturnLastEnemy();
            else if (target == Targeting.Strong) targetedEnemy = ReturnStrongestEnemy();
            else targetedEnemy = ReturnClosestEnemy();
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

    public Transform ReturnLastEnemy()
    {
        if (enemyList.Count == 0) return null;
        float minDistance = Mathf.Infinity;
        Transform last = null;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null) continue;
            Transform currentTransform = enemyList[i];
            float distance = currentTransform.GetComponent<EnemyPathfinding>().DistanceTraveled;
            if (distance < minDistance)
            {
                minDistance = distance;
                last = currentTransform;
            }
        }
        return last;
    }

    public Transform ReturnStrongestEnemy()
    {
        if (enemyList.Count == 0) return null;
        float highestRank = -1;
        Transform strongest = null;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null) continue;
            Transform currentTransform = enemyList[i];
            int rank = currentTransform.GetComponent<EnemyPathfinding>().Rank;
            if (rank > highestRank)
            {
                highestRank = rank;
                strongest = currentTransform;
            }
        }
        return strongest;
    }

    public Transform ReturnClosestEnemy()
    {
        if (enemyList.Count == 0) return null;
        float minDistance = Mathf.Infinity;
        Transform close = null;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null) continue;
            Transform currentTransform = enemyList[i];
            float distance = Vector3.Distance(currentTransform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                close = currentTransform;
            }
        }
        return close;
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
            if (GetComponent<TowerProjectile>()) tp.parent = GetComponent<TowerProjectile>().parent;
            else tp.parent = this;
        }
    }

    public IEnumerator SpawnProjectileDelayed()
    {
        attackWaiting = true;
        for (int i = 0; i < attackAmount; i++)
        {
            SpawnProjectile();
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
            if (nozzle) nozzle.LookAt(new Vector3(targetedEnemy.position.x, transform.position.y, targetedEnemy.position.z));
            if (currentFireTime <= 0 && !attackWaiting)
            {
                if (attackAmount <= 1) SpawnProjectile();
                else StartCoroutine(SpawnProjectileDelayed());
                currentFireTime = fireRate;
            }
        }

        //if (EnemyPathfinding.enemies.Count == 0) return;

        //Transform t;
        //if (target == Targeting.First) t = ReturnFurthestEnemy();
        //else if (target == Targeting.Last) t = ReturnLastEnemy();
        //else if (target == Targeting.Strong) t = ReturnStrongestEnemy();
        //else t = ReturnClosestEnemy();

        //potentialEnemy = t;

        //Vector3 targetedEnemy = t.position;
        //if (enemyList.Contains(t))
        //{
        //    if (nozzle) nozzle.LookAt(new Vector3(targetedEnemy.x, transform.position.y, targetedEnemy.z));
        //    if (currentFireTime <= 0 && !attackWaiting)
        //    {
        //        if (attackAmount <= 1) SpawnProjectile();
        //        else StartCoroutine(SpawnProjectileDelayed());
        //        currentFireTime = fireRate;
        //    }
        //}

    }

    public void ShootInRadius()
    {
        if (enemyList.Count > 0 && currentFireTime <= 0)
        {
            foreach (Transform t in enemyList)
            {
                EnemyHealth e = t.GetComponent<EnemyHealth>();

                float damageModifier = damage;
                if (element.sharp && e.element.bouncy || element.magic && e.element.hard) damageModifier *= 2;
                if (element.fire && e.element.grass || element.water && e.element.fire || element.electric && e.element.water || element.water) damageModifier *= 2;
                if (damageModifier >= e.Health) totalDamage += e.Health;
                else totalDamage += damageModifier;
                e.DecreaseHealth(damageModifier);
            }
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
