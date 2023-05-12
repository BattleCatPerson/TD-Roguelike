using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerButton : MonoBehaviour
{
    [SerializeField] TowerShoot tower;
    Button button;
    private void Start()
    {
        button = GetComponent<Button>();
    }
    public void ChangeTower()
    {
        SpawnTower.SetTower(tower);
    }

    private void Update()
    {
        button.interactable = SpawnTower.resources >= tower.Cost && HealthManager.health > 0;
    }
}
