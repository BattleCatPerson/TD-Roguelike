using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateProjectileOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    private void Start()
    {
        GetComponent<TowerProjectile>().onDestroy += Spawn;
    }

    public void Spawn()
    {
        var clone = Instantiate(projectile, transform.position, transform.rotation);
        clone.GetComponent<DamageEnemiesWithinTrigger>().parent = GetComponent<TowerProjectile>().parent;
    }
}
