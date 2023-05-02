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
    public static List<Transform> Path;
    [SerializeField] int seed;
    [SerializeField] List<GridRow> rows;
    [SerializeField] int numberOfInterestPoints;
    [SerializeField] List<Vector2Int> interestPoints;
    private int size;
    void Awake()
    {
        foreach (GridRow g in rows) AddTileToList(g.tiles);
        if (seed == 0) seed = (int)DateTime.Now.Ticks;
        UnityEngine.Random.InitState(seed);
        size = (int)Math.Sqrt(tiles.Count);
        grid = new Transform[size, size];
        path = new();
        Path = new();
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                grid[i, j] = tiles[count];
                count++;
            }
        }

        Vector2Int current = new();
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            current.x = 0;
            current.y = UnityEngine.Random.Range(0, size / 2);
        }
        else
        {
            current.x = UnityEngine.Random.Range(0, size / 2);
            current.y = 0;
        }
        startPosition = grid[current.x, current.y];
        Vector2Int destination = new();
        if (current.x == 0)
        {
            destination.x = size - 1;
            destination.y = UnityEngine.Random.Range(size / 2, size);
        }
        else
        {
            destination.x = UnityEngine.Random.Range(size / 2, size);
            destination.y = size - 1;
        }

        for (int i = 0; i < numberOfInterestPoints; i++)
        {
            Vector2Int interestPoint = new(UnityEngine.Random.Range(1, size), UnityEngine.Random.Range(1, size));
            interestPoints.Add(interestPoint);
            current = GeneratePath(current, interestPoint);
        }
        GeneratePath(current, destination);
    }

    void Update()
    {

    }


    public Vector2Int GeneratePath(Vector2Int currentPosition, Vector2Int destination)
    {
        path.Add(grid[currentPosition.x, currentPosition.y]);
        grid[currentPosition.x, currentPosition.y].GetComponent<MeshRenderer>().enabled = false;
        grid[currentPosition.x, currentPosition.y].GetComponent<TowerTile>().enabled = false;

        while (currentPosition != destination)
        {
            currentPosition += ReturnCloserDirection(currentPosition, destination);
            print(currentPosition.x + " " + currentPosition.y);
            path.Add(grid[currentPosition.x, currentPosition.y]);
            grid[currentPosition.x, currentPosition.y].GetComponent<MeshRenderer>().enabled = false;
            grid[currentPosition.x, currentPosition.y].GetComponent<TowerTile>().enabled = false;
        }
        Path.AddRange(path);
        return destination;
    }

    public Vector2Int ReturnCloserDirection(Vector2Int current, Vector2Int destination)
    {
        if (Math.Abs(destination.x - current.x) >= Math.Abs(destination.y - current.y)) return Vector2Int.right;
        return Vector2Int.up;
    }
    public void AddTileToList(List<Transform> t) => tiles.AddRange(t);
}
