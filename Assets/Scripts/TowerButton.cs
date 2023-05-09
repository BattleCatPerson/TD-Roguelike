using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] TowerShoot tower;

    public void ChangeTower()
    {
        SpawnTower.SetTower(tower);
    }
}
