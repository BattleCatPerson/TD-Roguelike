using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElement : MonoBehaviour
{
    [Header("Material")] public bool basic;
    public bool bouncy;
    public bool hard;

    [Header("Element")] public bool normal;
    public bool fire;
    public bool water;
    public bool electric;
    public bool grass;
}
