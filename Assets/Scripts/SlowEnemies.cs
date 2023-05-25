using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TowerShoot))]
public class SlowEnemies : MonoBehaviour
{
    [SerializeField] float slowPercentage;
    [SerializeField] float slowDuration;
    [SerializeField] TowerShoot tower;
    [SerializeField] List<EnemyPathfinding> slowedEnemies;
    void Start()
    {
        tower = GetComponent<TowerShoot>();
    }
    public void Slow()
    {
        foreach (Transform t in tower.EnemyList)
        {
            EnemyPathfinding e = t.GetComponent<EnemyPathfinding>();
            StartCoroutine(e.Slow(slowPercentage, slowDuration));
            slowedEnemies.Add(e);
        }
    }
}
