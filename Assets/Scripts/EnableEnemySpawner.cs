using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemySpawners;

    private void Start()
    {
        foreach (GameObject g in enemySpawners) g.SetActive(false);
        if (MapManager.Rounds != -1) enemySpawners[MapManager.Rounds - 1].SetActive(true);
    }
}
