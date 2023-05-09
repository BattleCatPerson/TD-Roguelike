using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    public bool isEnabled;
    [SerializeField] TowerShoot currentTower;

    public void SetTower(TowerShoot tower)
    {
        currentTower = tower;
        isEnabled = false;
    }
}
