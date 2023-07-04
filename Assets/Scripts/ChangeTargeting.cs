using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTargeting : MonoBehaviour
{
    [SerializeField] Targeting targeting;

    public void Change()
    {
        if (InspectTower.clickedTower) InspectTower.clickedTower.target = targeting;
    }
}
