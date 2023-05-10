using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(SpawnTower))]
public class UpdateText : MonoBehaviour
{
    SpawnTower s;
    [SerializeField] TextMeshProUGUI resourcesText;
    void Start()
    {
        s = GetComponent<SpawnTower>();
    }

    void Update()
    {
        resourcesText.text = "Resources: " + SpawnTower.resources;
    }
}
