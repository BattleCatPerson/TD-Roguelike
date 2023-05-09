using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : MonoBehaviour
{
    [SerializeField] float resources;
    static TowerShoot currentTower;
    [SerializeField] TowerShoot currentTowerInspector;
    [SerializeField] float currentCost;
    [SerializeField] TowerTile hoveredTile;
    public static void SetTower(TowerShoot tower)
    {
        currentTower = tower;
    }

    private void Update()
    {
        if (currentTower != null)
        {
            currentTowerInspector = currentTower;
            currentCost = currentTower.Cost;
        }
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent<TowerTile>(out TowerTile t))
            {
                hoveredTile = t;
            }
        }
    }
}
