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
    [SerializeField] LayerMask tileLayer;
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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer) && hit.collider.GetComponent<TowerTile>() && hit.collider.GetComponent<TowerTile>().isEnabled)
        {
            hoveredTile = hit.collider.GetComponent<TowerTile>();
        }
        else hoveredTile = null;
    }
}
