using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : MonoBehaviour
{
    public static SpawnTower instance;
    public static List<float> resources;
    static TowerShoot currentTower;
    [SerializeField] TowerShoot currentTowerInspector;
    [SerializeField] List<float> currentCost;
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
        if (stop || EnemySpawner.doneSpawning) return;
        if (currentTower != null)
        {
            currentTowerInspector = currentTower;
            currentCost = currentTower.ResourceCosts;
        }

        RaycastTile();
        if (Input.GetMouseButtonDown(0) && currentTower)
        {
            if (hoveredTile)
            {
                List<float> c = buildPhase ? currentTower.ResourceCosts : currentTower.ResourceCostsAfterBuildPhase;
                hoveredTile.SpawnTower(currentTower);
                for (int i = 0; i < resources.Count; i++) resources[i] -= c[i];
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
        if (stop) return;
        stop = true;
        if (towerPanel) towerPanel.SetActive(false);
        if (statusPanel) statusPanel.SetActive(false);
    }

    public void StopBuildPhase()
    {
        buildPhase = false;
    }
}
