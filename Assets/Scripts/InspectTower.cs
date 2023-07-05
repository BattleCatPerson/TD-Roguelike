using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using TMPro;
using Unity.VisualScripting;

public class InspectTower : MonoBehaviour
{
    public static TowerShoot clickedTower;
    [SerializeField] TowerShoot ClickedTower;
    [SerializeField] TowerShoot hoveredTower;
    [SerializeField] GameObject range;

    [SerializeField] List<TextMeshProUGUI> panelTexts;
    [SerializeField] List<GameObject> targetingElements;
    private bool stop;

    private void Awake()
    {
        HealthManager.onDeath += OnDeath;
    }

    void Start()
    {
        clickedTower = null;
        foreach (GameObject g in targetingElements) g.SetActive(false);
    }

    void Update()
    {
        if (stop || EnemySpawner.doneSpawning)
        {
            DisableComponents();
            return;
        }
        range.SetActive(clickedTower);
        ClickedTower = clickedTower;

        if (Input.GetMouseButtonDown(0))
        {
            if (hoveredTower) EnableComponents();
            else if (!HoverUI.hoveringUI) DisableComponents();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.GetComponentInParent<TowerShoot>()) hoveredTower = hit.collider.GetComponentInParent<TowerShoot>();
        else hoveredTower = null;

        if (clickedTower)
        {
            targetingElements[0].GetComponent<TextMeshProUGUI>().text = "Targeting: " + clickedTower.target;
            panelTexts[1].text = "Damage: " + clickedTower.TotalDamage;

        }

    }

    public void OnDeath() => stop = true;

    public void EnableComponents()
    {
        if (hoveredTower.TargetEnemiesOrAttackInRadius) foreach (GameObject g in targetingElements) g.SetActive(true);
        clickedTower = hoveredTower;
        range.transform.localScale = new(clickedTower.Range * 2, range.transform.localScale.y, clickedTower.Range * 2);
        range.transform.position = clickedTower.transform.position;
        panelTexts[0].text = clickedTower.name.Substring(0, clickedTower.name.Length - 7);
    }

    public void DisableComponents()
    {
        clickedTower = null;
        range.SetActive(false);
        foreach (GameObject g in targetingElements) g.SetActive(false);
        foreach (TextMeshProUGUI t in panelTexts) t.text = "";
    }
}
