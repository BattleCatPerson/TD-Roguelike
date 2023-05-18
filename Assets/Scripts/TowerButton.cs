using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerButton : MonoBehaviour
{
    [SerializeField] TowerShoot tower;
    Button button;
    TextMeshProUGUI text;
    private void Start()
    {
        button = GetComponent<Button>();
        text = button.GetComponentInChildren<TextMeshProUGUI>();
        text.text = tower.name;
    }
    public void ChangeTower()
    {
        SpawnTower.SetTower(tower);
    }

    private void Update()
    {
        var r = SpawnTower.instance.BuildPhase ? tower.ResourceCosts : tower.ResourceCostsAfterBuildPhase;

        for (int i = 0; i < SpawnTower.resources.Count; i++)
        {
            if (SpawnTower.resources[i] < r[i])
            {
                button.interactable = false;
                break;
            }
        }

        if (HealthManager.health <= 0) button.interactable = false;
    }
}
