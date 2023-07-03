using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TowerProjectile : MonoBehaviour
{
    [SerializeField] float damage;
    public float Damage => damage;
    [SerializeField] float lifetime;
    [SerializeField, Header("Set colliders to triggers for piercing")] int pierce;

    public event Action onDestroy;

    public TowerShoot parent;

    int collisionCounter;
    private void Start()
    {
        collisionCounter = 0;
        Destroy(gameObject, lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        collisionCounter++;
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth e) && collisionCounter == 1) DealDamage(e);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth e))
        {
            DealDamage(e);
            pierce--;
        }
        if (pierce == 0) Destroy(gameObject);
    }

    public void SetDamage(float d) => damage = d;

    private void OnDestroy() => onDestroy?.Invoke();

    public void DealDamage(EnemyHealth e)
    {
        float damageModifier = damage;
        if (parent.element.sharp && e.element.bouncy || parent.element.magic && e.element.hard) damageModifier *= 2;
        if (parent.element.fire && e.element.grass || parent.element.water && e.element.fire || parent.element.electric && e.element.water || parent.element.water) damageModifier *= 2;
        if (damageModifier >= e.Health)
        {
            print(e.Health);
            parent.TotalDamage += e.Health;
            e.DecreaseHealth(e.Health);
        }
        else
        {
            parent.TotalDamage += damageModifier;
            e.DecreaseHealth(damageModifier);
        }
    }
}
