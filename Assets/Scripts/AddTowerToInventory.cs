using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddTowerToInventory : MonoBehaviour
{
    public static List<TowerButton> commonTowersStatic = new();
    [SerializeField] List<TowerButton> commonTowers;
    //[SerializeField] List<TowerButton> rareTowers;
    //[SerializeField] List<TowerButton> legendaryTowers;
    [SerializeField] TowerButton selectedTower;
    [SerializeField] bool added;
    private void Awake()
    {
        if (commonTowersStatic.Count == 0) commonTowersStatic = commonTowers;
    }
    void Start()
    {
        commonTowers = commonTowersStatic;
        //todo: generate random rarity alongside random tower in that list
        if (commonTowers.Count == 0) return;
        int index = Random.Range(0, commonTowers.Count);
        selectedTower = commonTowers[index];
    }
    
    public void AddToInventory()
    {
        if (added) return;
        TowerInventory.inventory.Add(selectedTower);
        added = true;
        commonTowersStatic.Remove(selectedTower);
        MapManager.instance.LoadBattleScene(GetComponent<Button>());
    }
}
