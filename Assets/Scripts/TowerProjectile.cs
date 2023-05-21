using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TowerProjectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float lifetime;
    [SerializeField, Header("Set colliders to triggers for piercing")] int pierce;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth e)) e.DecreaseHealth(damage);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth e))
        {
            e.DecreaseHealth(damage);
            pierce--;
        }
        if (pierce == 0) Destroy(gameObject);
    }

    public void SetDamage(float d) => damage = d;
}
