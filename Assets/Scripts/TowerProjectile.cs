using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TowerProjectile : MonoBehaviour
{
    public static List<GameObject> activeProjectiles = new();

    [SerializeField] float damage;
    public float Damage => damage;
    [SerializeField] float lifetime;
    [SerializeField, Header("Set colliders to triggers for piercing")] int pierce;

    [SerializeField] bool isWall;
    [SerializeField] float health;
    public event Action onDestroy;

    public TowerShoot parent;
    public Transform tile;
    int collisionCounter;
    private void Start()
    {
        activeProjectiles.Add(gameObject);
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
            if (isWall)
            {
                if (health <= e.Health)
                {
                    DealDamage(e);
                    Destroy(gameObject);
                }
                else
                {
                    float healthAfter = health - e.Health;
                    DealDamage(e);
                    health = healthAfter;
                }
            }
            else
            {
                DealDamage(e);
                pierce--;
                if (pierce == 0) Destroy(gameObject);
            }
        }
    }

    public void SetDamage(float d) => damage = d;
    public void SetHealth(float h) => health = h;

    private void OnDestroy()
    {
        onDestroy?.Invoke();
        activeProjectiles.Remove(gameObject);
        if (tile) parent.takenTiles.Remove(tile);
    }

    public void DealDamage(EnemyHealth e)
    {
        float damageModifier = damage;
        if (isWall) damageModifier = health;
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
}
