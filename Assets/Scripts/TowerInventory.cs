using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInventory : MonoBehaviour
{
    public static List<TowerButton> inventory = new();
    [SerializeField] List<TowerButton> Inventory;
    private void Awake()
    {
        if (inventory.Count > 0)
        {
            foreach (TowerButton t in inventory)
            {
                var clone = Instantiate(t, transform);
            }
        }
        Inventory = inventory;
    }
}
