using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddResources : MonoBehaviour
{
    [SerializeField] float resources;
    [SerializeField] bool pressed;
    public void Add()
    {
        if (pressed) return;
        pressed = true;
        SpawnTower.resources += resources;
        MapManager.instance.LoadBattleScene(GetComponent<Button>());
    }
}
