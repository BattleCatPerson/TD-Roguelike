using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamageEnemiesWithinTrigger : MonoBehaviour
{
    [SerializeField] float damagePerInterval;
    [SerializeField] float timeInterval;
    [SerializeField] List<EnemyHealth> enemies;
    [SerializeField] float lifeTime;

    public TowerShoot parent;
    [SerializeField] float currentTimeInterval;
    private void Start()
    {
        TowerProjectile.activeProjectiles.Add(gameObject);
        Destroy(gameObject, lifeTime);
        currentTimeInterval = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth e) && !enemies.Contains(e)) enemies.Add(e);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth e) && enemies.Contains(e)) enemies.Remove(e);
    }

    private void Update()
    {
        if (currentTimeInterval <= 0)
        {
            foreach (EnemyHealth e in enemies)
            {
                float damageModifier = damagePerInterval;
                if (parent.element.sharp && e.element.bouncy || parent.element.magic && e.element.hard) damageModifier *= 2;
                if (parent.element.fire && e.element.grass || parent.element.water && e.element.fire || parent.element.electric && e.element.water || parent.element.water) damageModifier *= 2;
                if (damageModifier >= e.Health)
                {
                    parent.TotalDamage += e.Health;
                    e.DecreaseHealth(e.Health);
                }
                else
                {
                    parent.TotalDamage += damageModifier;
                    e.DecreaseHealth(damageModifier);
                }
            }
            currentTimeInterval = timeInterval;
        }
        else currentTimeInterval -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        TowerProjectile.activeProjectiles.Remove(gameObject);
    }
}
