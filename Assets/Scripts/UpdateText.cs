using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpdateText : MonoBehaviour
{
    SpawnTower s;
    [SerializeField] List<TextMeshProUGUI> texts;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] List<string> nameOrder;
    void Start()
    {
        s = GetComponent<SpawnTower>();
    }

    void Update()
    {
        if (nameOrder.Count == 0) return;
        for (int i = 0; i < SpawnTower.resources.Count; i++) texts[i].text = nameOrder[i] + ": " + SpawnTower.resources[i];

        if (!descriptionText) return;
        if (HoverUI.hoveredTower == null)
        {
            descriptionText.text = "";
        }
        else
        {
            descriptionText.text = "";
            TowerShoot tower = HoverUI.hoveredTower.Tower;
            List<float> costs = SpawnTower.instance.BuildPhase ? tower.ResourceCosts : tower.ResourceCostsAfterBuildPhase;
            for (int i = 0; i < costs.Count - 1; i++) descriptionText.text += costs[i] + " " + nameOrder[i] + ", ";
            descriptionText.text += costs[costs.Count - 1] + " " + nameOrder[costs.Count - 1];
        }

    }
}
