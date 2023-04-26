using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvePath : MonoBehaviour
{
    [SerializeField] List<Transform> rows;
    Transform[,] grid;
    void Start()
    {
        grid = new Transform[rows.Count, rows.Count];
    }

    void Update()
    {
        
    }
}
