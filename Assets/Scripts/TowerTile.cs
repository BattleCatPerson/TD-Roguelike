using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    public bool isEnabled;
    [SerializeField] TowerShoot currentTower;
    public TowerShoot CurrentTower => currentTower;
    [SerializeField] Transform towerSpawn;

    public void SpawnTower(TowerShoot tower)
    {
        var clone = Instantiate(tower.gameObject, new(transform.position.x, towerSpawn.position.y, transform.position.z), towerSpawn.rotation);
        isEnabled = false;
        currentTower = clone.GetComponent<TowerShoot>();
    }
}
