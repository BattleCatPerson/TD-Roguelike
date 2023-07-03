using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using TMPro;
public class InspectTower : MonoBehaviour
{
    public static TowerShoot clickedTower;
    [SerializeField] TowerShoot ClickedTower;
    [SerializeField] TowerShoot hoveredTower;
    [SerializeField] GameObject range;

    [SerializeField] List<TextMeshProUGUI> panelTexts;

    private bool stop;

    private void Awake()
    {
        HealthManager.onDeath += OnDeath;
    }

    void Start()
    {
        clickedTower = null;
    }

    void Update()
    {
        if (stop || EnemySpawner.doneSpawning) return;
        range.SetActive(clickedTower);
        ClickedTower = clickedTower;

        if (Input.GetMouseButtonDown(0))
        {
            if (hoveredTower) clickedTower = hoveredTower;
            else
            {
                clickedTower = null;
                range.SetActive(false);
                foreach (TextMeshProUGUI t in panelTexts) t.text = "";
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.GetComponentInParent<TowerShoot>()) hoveredTower = hit.collider.GetComponentInParent<TowerShoot>();
        else hoveredTower = null;

        

        if (clickedTower)
        {
            range.transform.localScale = new(clickedTower.Range * 2, range.transform.localScale.y, clickedTower.Range * 2);
            range.transform.position = clickedTower.transform.position;
            panelTexts[0].text = clickedTower.name.Substring(0, clickedTower.name.Length - 7);
            panelTexts[1].text = "Damage: " + clickedTower.TotalDamage;
        }
    }

    public void OnDeath() => stop = true;
}
