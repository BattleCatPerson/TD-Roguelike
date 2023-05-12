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
        text.text = tower.name + " " + tower.Cost;
    }
    public void ChangeTower()
    {
        SpawnTower.SetTower(tower);
    }

    private void Update()
    {
        text.text = tower.name + " " + tower.Cost;
        button.interactable = SpawnTower.resources >= tower.Cost && HealthManager.health > 0;
    }
}
