using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvePath : MonoBehaviour
{
    [SerializeField] List<Transform> tiles;
    Transform[,] grid;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] List<Transform> path;
    [SerializeField] int seed;
    [SerializeField] List<GridRow> rows;
    private int size;
    void Start()
    {
        foreach (GridRow g in rows) AddTileToList(g.tiles);
        if (seed == 0) seed = (int)DateTime.Now.Ticks;
        UnityEngine.Random.InitState(seed);
        size = (int) Math.Sqrt(tiles.Count);
        grid = new Transform[size, size];
        path = new List<Transform>();

        GeneratePath();
    }

    void Update()
    {
        
    }

    public void GeneratePath()
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                grid[i, j] = tiles[count];
                count++;
            }
        }
        foreach (Transform t in grid)
        {
            print(t);
        }

        int x, y;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            x = 0;
            y = UnityEngine.Random.Range(0, size / 2);
        }
        else
        {
            x = UnityEngine.Random.Range(0, size / 2);
            y = 0;
        }
        startPosition = grid[x, y];

        int x2, y2;
        if (x == 0)
        {
            x2 = size - 1;
            y2 = UnityEngine.Random.Range(size / 2, size);
        }
        else
        {
            x2 = UnityEngine.Random.Range(size / 2, size);
            y2 = size - 1;
        }
        endPosition = grid[x2, y2];

        Vector2Int currentPositon = new(x, y);

        grid[currentPositon.x, currentPositon.y].gameObject.SetActive(false);
        while (currentPositon != new Vector2(x2, y2))
        {
            currentPositon += ReturnCloserDirection(currentPositon, x2, y2);
            grid[currentPositon.x, currentPositon.y].gameObject.SetActive(false);
            print(currentPositon);
        }
    }

    public Vector2Int ReturnCloserDirection(Vector2Int current, int x, int y)
    {
        if (Math.Abs(x - current.x) >= Math.Abs(y - current.y)) return Vector2Int.right;
        return Vector2Int.up;
    }
    public void AddTileToList(List<Transform> t) => tiles.AddRange(t);
}
