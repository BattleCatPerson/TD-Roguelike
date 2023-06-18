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
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth e))
        {
            float damageModifier = damage;
            if (parent.element.sharp && e.element.bouncy || parent.element.magic && e.element.hard) damageModifier *= 2;
            if (parent.element.fire && e.element.grass || parent.element.water && e.element.fire || parent.element.electric && e.element.water || parent.element.water) damageModifier *= 2;
            e.DecreaseHealth(damageModifier);
            print(damageModifier);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth e))
        {
            float damageModifier = damage;
            if (parent.element.sharp && e.element.bouncy || parent.element.magic && e.element.hard) damageModifier *= 2;
            if (parent.element.fire && e.element.grass || parent.element.water && e.element.fire || parent.element.electric && e.element.water || parent.element.water) damageModifier *= 2;
            e.DecreaseHealth(damageModifier);
            print(damageModifier);
            pierce--;
        }
        if (pierce == 0) Destroy(gameObject);
    }

    public void SetDamage(float d) => damage = d;

    private void OnDestroy() => onDestroy?.Invoke();
}
