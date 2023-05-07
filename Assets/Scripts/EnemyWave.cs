using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "New Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    public List<EnemyPathfinding> enemies;
    public float timeBetweenEnemies;
}
