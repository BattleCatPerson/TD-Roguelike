using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : MonoBehaviour
{
    public static float resources;
    [SerializeField] float startingResources;
    static TowerShoot currentTower;
    [SerializeField] TowerShoot currentTowerInspector;
    [SerializeField] float currentCost;
    [SerializeField] TowerTile hoveredTile;
    [SerializeField] LayerMask tileLayer;

    [SerializeField] GameObject towerPanel;
    [SerializeField] GameObject statusPanel;
    bool stop;
    private void Awake()
    {
        resources = startingResources;
        HealthManager.onDeath += OnDeath;
    }

    public static void SetTower(TowerShoot tower)
    {
        currentTower = tower;
    }

    private void Update()
    {
        if (stop) return;
        if (currentTower != null)
        {
            currentTowerInspector = currentTower;
            currentCost = currentTower.Cost;
        }

        RaycastTile();
        if (hoveredTile && Input.GetMouseButtonDown(0) && currentTower)
        {
            hoveredTile.SpawnTower(currentTower);
            resources -= currentTower.Cost;
            currentTower = null;
        }
    }

    public void RaycastTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer) && hit.collider.GetComponent<TowerTile>() && hit.collider.GetComponent<TowerTile>().isEnabled)
        {
            hoveredTile = hit.collider.GetComponent<TowerTile>();
        }
        else hoveredTile = null;
    }

    public void OnDeath()
    {
        stop = true;
        towerPanel.SetActive(false);
        statusPanel.SetActive(false);
    }
}
