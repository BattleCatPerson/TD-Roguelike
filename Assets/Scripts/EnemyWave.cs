using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "Enemy Wave/New Enemy Wave", order = 1)]
public class EnemyWave : ScriptableObject
{
    public List<EnemyPathfinding> enemies;
    public float timeBetweenEnemies;
}
