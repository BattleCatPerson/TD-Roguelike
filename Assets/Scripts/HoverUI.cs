using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HoverUI : MonoBehaviour
{
    [SerializeField] GraphicRaycaster raycaster;
    public static TowerButton hoveredTower;
    public static bool hoveringUI;
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        hoveredTower = null;
        var p = new PointerEventData(EventSystem.current);
        p.position = Input.mousePosition;
        List<RaycastResult> r = new();
        raycaster.Raycast(p, r);
        hoveringUI = r.Count > 0;
        foreach (RaycastResult result in r)
        {
            if (result.gameObject.TryGetComponent<TowerButton>(out TowerButton t))
            {
                hoveredTower = t;
                break;
            }
        }
    }
}
