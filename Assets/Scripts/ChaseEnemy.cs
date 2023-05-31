using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TowerShoot))]
public class ChaseEnemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float shootDistance;
    [SerializeField] float moveDistance;
    [SerializeField] Transform target;
    [SerializeField] float distance;
    bool shooting;
    bool chasing;
    bool moving;
    TowerShoot shoot;
    void Start()
    {
        shoot = GetComponent<TowerShoot>();
        moving = true;
        shoot.SetDamage(GetComponent<TowerProjectile>().Damage);
    }

    void Update()
    {
        target = shoot.TargetedEnemy;
        if (target)
        {
            distance = Vector3.Distance(transform.position, target.position);
            if (distance > shootDistance && moving)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                shooting = false;
            }
            else
            {
                shooting = true;
                moving = false;
            }

            if (distance > moveDistance && shooting)
            {
                moving = true;
                shooting = false;
            }
        }

    }
}
