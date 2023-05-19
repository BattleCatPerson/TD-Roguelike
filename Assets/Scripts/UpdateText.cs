using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(SpawnTower))]
public class UpdateText : MonoBehaviour
{
    SpawnTower s;
    [SerializeField] List<TextMeshProUGUI> texts;
    [SerializeField] List<string> nameOrder;
    [SerializeField] GameObject hoveredUI;
    // use graphicsraycast
    void Start()
    {
        s = GetComponent<SpawnTower>();
    }

    void Update()
    {
        if (nameOrder.Count == 0) return;
        for (int i = 0; i < SpawnTower.resources.Count; i++) texts[i].text = nameOrder[i] + ": " + SpawnTower.resources[i];
    }

    public bool hoveringUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
