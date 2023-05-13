using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : MonoBehaviour
{
    public static SpawnTower instance;
    public static float resources;
    static TowerShoot currentTower;
    [SerializeField] TowerShoot currentTowerInspector;
    [SerializeField] float currentCost;
    [SerializeField] TowerTile hoveredTile;
    [SerializeField] LayerMask tileLayer;

    [SerializeField] GameObject towerPanel;
    [SerializeField] GameObject statusPanel;

    [SerializeField] bool buildPhase;
    public bool BuildPhase { get { return buildPhase; } }
    private bool stop;
    private void Awake()
    {
        instance = this;
        HealthManager.onDeath += OnDeath;
        buildPhase = true;
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
        if (Input.GetMouseButtonDown(0) && currentTower)
        {
            if (hoveredTile)
            {
                float c = buildPhase ? currentTower.Cost : currentTower.CostAfterBuildPhase;
                hoveredTile.SpawnTower(currentTower);
                resources -= c;
            }
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

    public void StopBuildPhase()
    {
        buildPhase = false;
    }
}
