using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElement : MonoBehaviour
{
    [Header("Damage Type")] public bool regular;
    public bool sharp;
    public bool magic;

    [Header("Element")] public bool normal;
    public bool fire;
    public bool water;
    public bool electric;
    public bool grass;
}
