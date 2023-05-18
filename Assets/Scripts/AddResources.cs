using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddResources : MonoBehaviour
{
    [SerializeField] List<float> resources;
    [SerializeField] bool pressed;
    public void Add()
    {
        if (pressed) return;
        pressed = true;
        for (int i = 0; i < resources.Count; i++) SpawnTower.resources[i] += resources[i];
        MapManager.instance.LoadBattleScene(GetComponent<Button>());
    }
}
