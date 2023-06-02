using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamageEnemiesWithinTrigger : MonoBehaviour
{
    [SerializeField] float damagePerSecond;
    [SerializeField] List<EnemyHealth> enemies;
    [SerializeField] float lifeTime;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
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
        foreach (EnemyHealth e in enemies) e.DecreaseHealth(damagePerSecond * Time.deltaTime);
    }
}
