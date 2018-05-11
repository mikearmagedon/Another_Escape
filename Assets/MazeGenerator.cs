using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] MazeCell cellPrefab;

    private MazeCell[,] cells;

    // Use this for initialization
    void Start()
	{
        InitializeMaze();

        MazeAlgorithm ma = new BinaryTreeAlgorithm(cells);
        ma.CreateMaze();
	}

    private void InitializeMaze()
    {
        cells = new MazeCell[size.x, size.y];

        for (int r = 0; r < size.x; r++)
        {
            for (int c = 0; c < size.y; c++)
            {
                CreateCell(r, c);
            }
        }
    }

    private void CreateCell(int r, int c)
    {
        // Instantiate Cell
        MazeCell newCell = Instantiate(cellPrefab, transform) as MazeCell;
        newCell.name = r + "," + c;
        newCell.transform.position = new Vector3(r * MazeCell.cellSize, 0f, c * MazeCell.cellSize);

        // Add to the maze
        cells[r, c] = newCell;
        return;
    }
}
