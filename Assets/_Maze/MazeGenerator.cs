﻿using System;
using System.Collections;
using UnityEngine;

public enum Algorithm {BinaryTree, Sidewinder};

public class MazeGenerator : MonoBehaviour
{
    public Vector2Int initialCoordinates;
    public Vector2Int size;
    public Algorithm mazeAlgorithm;
    public float secondsBetweenGenerations;
    public Vector2Int mazeEntrance;
    public MazeDirection mazeEntranceDirection;
    public Vector2Int mazeExit;
    public MazeDirection mazeExitDirection;

    [SerializeField] MazeCell cellPrefab;

    private MazeCell[,] cells;
    private MazeAlgorithm ma;

    void Start()
    {
        InitializeMaze();
        ConfigureCells();
        StartAlgorithm();
        StartCoroutine(ContinuousMazeGeneration());
    }

    private void StartAlgorithm()
    {
        switch (mazeAlgorithm)
        {
            case Algorithm.BinaryTree:
                ma = new BinaryTreeAlgorithm(cells);
                break;
            case Algorithm.Sidewinder:
                ma = new SidewinderAlgorithm(cells);
                break;
            default:
                Debug.Log("Unknown algorithm.");
                break;
        }
    }

    // TODO consider using a bool argument to set the continuous generation
    public IEnumerator ContinuousMazeGeneration()
    {
        CreateMazeEntrance(mazeEntrance, mazeEntranceDirection);
        CreateMazeExit(mazeExit, mazeExitDirection);
        ma.CreateMaze();
        // start maze clean up
        CleanOverlappingMazeWalls();
        yield return new WaitForSeconds(secondsBetweenGenerations);
        ResetMaze();
        StartCoroutine(ContinuousMazeGeneration());
    }

    private void CleanOverlappingMazeWalls()
    {
        foreach (var cell in cells)
        {
            foreach (var neighbour in cell.Neighbors)
            {
                if (!cell.IsLinked(neighbour))
                {
                    neighbour.CreatePassage(cell, false);
                }
            }
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
        newCell.coordinates = coordinates + initialCoordinates;
        newCell.name = (coordinates.x + initialCoordinates.x) + "," + (coordinates.y + initialCoordinates.y);
        newCell.transform.position = new Vector3((coordinates.x + initialCoordinates.x) * MazeCell.cellSize, 0f, (coordinates.y + initialCoordinates.y) * MazeCell.cellSize);

        // Add to the maze
        cells[coordinates.x, coordinates.y] = newCell;
        return;
    }

    /**
     * Assign the neighbours of each cell
     */
    private void ConfigureCells()
    {
        foreach (var cell in cells)
        {
            int c = cell.coordinates.x - initialCoordinates.x;
            int r = cell.coordinates.y - initialCoordinates.y;

            if (r + 1 < size.y)
            {
                cell.North = cells[c, r + 1];
            }
            if (c + 1 < size.x)
            {
                cell.East = cells[c + 1, r];
            }
            if (r - 1 >= 0)
            {
                cell.South = cells[c, r - 1];
            }
            if (c - 1 >= 0)
            {
                cell.West = cells[c - 1, r];
            }
        }
    }

    private void ResetMaze()
    {
        foreach (var cell in cells)
        {
            cell.Reset();
        }
    }

    private void CreateMazeEntrance(Vector2Int coordinates, MazeDirection direction)
    {
        cells[coordinates.x, coordinates.y].entrance = direction;
    }

    private void CreateMazeExit(Vector2Int coordinates, MazeDirection direction)
    {
        cells[coordinates.x, coordinates.y].exit = direction;
    }
}
