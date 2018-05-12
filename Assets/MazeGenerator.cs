using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] MazeCell cellPrefab;
    [SerializeField] float secondsBetweenGenerations = 5f;

    private MazeCell[,] cells;
    private MazeAlgorithm ma;

    // Use this for initialization
    void Start()
    {
        InitializeMaze();
        ma = new BinaryTreeAlgorithm(cells);
        StartCoroutine(TimeKeeper());
    }

    IEnumerator TimeKeeper()
    {
        while (true)
        {
            foreach (var cell in cells)
            {
                cell.Reset();
            }
            yield return new WaitForSeconds(secondsBetweenGenerations);

            StartCoroutine(ma.CreateMaze());

        }
    }

    private void InitializeMaze()
    {
        cells = new MazeCell[size.x, size.y];

        for (int r = 0; r < size.x; r++)
        {
            for (int c = 0; c < size.y; c++)
            {
                CreateCell(new Vector2Int(c, r));
            }
        }
    }

    private void CreateCell(Vector2Int coordinates)
    {
        // Instantiate Cell
        MazeCell newCell = Instantiate(cellPrefab, transform) as MazeCell;
        newCell.coordinates = coordinates;
        newCell.name = coordinates.x + "," + coordinates.y;
        newCell.transform.position = new Vector3(coordinates.x * MazeCell.cellSize, 0f, coordinates.y * MazeCell.cellSize);

        // Add to the maze
        cells[coordinates.x, coordinates.y] = newCell;
        return;
    }
}
