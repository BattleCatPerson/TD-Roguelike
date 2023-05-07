using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TowerProjectile : MonoBehaviour
{
    [SerializeField] float damage;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth e)) e.DecreaseHealth(damage);
    }

    public void SetDamage(float d) => damage = d;
}
