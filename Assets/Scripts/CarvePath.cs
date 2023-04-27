using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvePath : MonoBehaviour
{
    [SerializeField] List<Transform> transforms;
    Transform[,] grid;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;

    void Start()
    {
        grid = new Transform[transforms.Count, transforms.Count];
        int count = 0;
        for (int i = 0; i < transforms.Count; i++)
        {
            for (int j = 0; j < transforms.Count; j++)
            {
                grid[i, j] = transforms[count];
                count++;
            }
        }
    }

    void Update()
    {
        
    }
}
