using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CarvePath : MonoBehaviour
{
    [SerializeField] List<Transform> tiles;
    Transform[,] grid;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] List<Transform> path;
    [SerializeField] List<Transform> overlapping;
    public static List<Transform> Path;
    [SerializeField] int seed;
    [SerializeField] List<GridRow> rows;
    [SerializeField] int numberOfInterestPoints;
    [SerializeField] List<Transform> interestPoints;
    [SerializeField] Material gradient;
    [SerializeField] Material endGradient;
    private int size;
    void Awake()
    {
        InitializeGrid();
    }

    public Vector2Int GeneratePath(Vector2Int currentPosition, Vector2Int destination)
    {
        Transform currentTransform = grid[currentPosition.y, currentPosition.x];
        if (path.Contains(currentTransform)) overlapping.Add(currentTransform);
        path.Add(currentTransform);
        Path.Add(currentTransform);
        currentTransform.GetComponent<TowerTile>().enabled = false;
        while (currentPosition != destination)
        {
            currentPosition += ReturnCloserDirection(currentPosition, destination);
            if (currentPosition.x >= size || currentPosition.y >= size) break;
            currentTransform = grid[currentPosition.y, currentPosition.x];
            if (path.Contains(currentTransform)) overlapping.Add(currentTransform);
            path.Add(currentTransform);
            Path.Add(currentTransform);
            currentTransform.GetComponent<TowerTile>().enabled = false;
        }
        path.Add(currentTransform);
        Path.Add(currentTransform);
        currentTransform.GetComponent<TowerTile>().enabled = false;
        return destination;
    }

    public Vector2Int ReturnCloserDirection(Vector2Int current, Vector2Int destination)
    {
        if (Math.Abs(destination.x - current.x) >= Math.Abs(destination.y - current.y))
        {
            Vector2Int dir = destination.x - current.x > 0 ? Vector2Int.right : Vector2Int.left;
            return dir;
        }
        else
        {
            Vector2Int dir = destination.y - current.y > 0 ? Vector2Int.up : Vector2Int.down;
            return dir;
        }
    }
    public void AddTileToList(List<Transform> t) => tiles.AddRange(t);

    public void AssignColor()
    {
        Vector3 colorDelta = new(endGradient.color.r - gradient.color.r, endGradient.color.g - gradient.color.g, endGradient.color.b - gradient.color.b);
        float amountPerTile = 1f / Path.Count;
        float total = 0f;
        foreach (Transform t in Path)
        {
            Material m = new Material(gradient);
            Color c = new(m.color.r + colorDelta.x * total, m.color.g + colorDelta.y * total, m.color.b + colorDelta.z * total, 1 - total); //make sure the material's set to transparent so it can be transparent!
            m.color = c;
            total += amountPerTile;
            t.GetComponent<Renderer>().material = m;
        }
    }

    public void InitializeGrid()
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
            Vector2Int interestPoint = new(UnityEngine.Random.Range(1, size - 1), UnityEngine.Random.Range(1, size - 1));
            interestPoints.Add(grid[interestPoint.y, interestPoint.x]);
            current = GeneratePath(current, interestPoint);
        }

        GeneratePath(current, destination);
        AssignColor();

    }
}
