using System.Collections;
using UnityEditor;
using UnityEngine;

public enum Algorithm {BinaryTree, Sidewinder, HuntAndKill, GrowingTree};

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector3Int initialCoordinates;
    [SerializeField] Vector2Int size;
    [SerializeField] Algorithm mazeAlgorithm;
    [SerializeField] bool continuosGeneration;
    [SerializeField] float secondsBetweenGenerations;
    [SerializeField] Vector2Int mazeEntrance;
    [SerializeField] MazeDirection mazeEntranceDirection;
    [SerializeField] Vector2Int mazeExit;
    [SerializeField] MazeDirection mazeExitDirection;
    [SerializeField] MazeCell cellPrefab;

    private MazeCell[,] cells;
    private MazeAlgorithm ma;

    void Start()
    {
        InitializeMaze();
        ConfigureCells();
        StartAlgorithm();
        StartCoroutine(ContinuousMazeGeneration(continuosGeneration));
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
            case Algorithm.HuntAndKill:
                ma = new HuntAndKillAlgorithm(cells);
                break;
            case Algorithm.GrowingTree:
                ma = new GrowingTreeAlgorithm(cells);
                break;
            default:
                Debug.Log("Unknown algorithm.");
                break;
        }
    }

    public IEnumerator ContinuousMazeGeneration(bool continuousGeneration)
    {
        CreateMazeEntrance(mazeEntrance, mazeEntranceDirection);
        CreateMazeExit(mazeExit, mazeExitDirection);
        ma.CreateMaze();
        CleanOverlappingMazeWalls();

        if (continuousGeneration)
        {
            yield return new WaitForSeconds(secondsBetweenGenerations);
            ResetMaze();
            StartCoroutine(ContinuousMazeGeneration(true));
        }
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

        for (int r = 0; r < size.y; r++)
        {
            for (int c = 0; c < size.x; c++)
            {
                CreateCell(new Vector2Int(c, r));
            }
        }
    }

    private void CreateCell(Vector2Int coordinates)
    {
        // Instantiate Cell
        MazeCell newCell = Instantiate(cellPrefab, transform) as MazeCell;
        newCell.coordinates = new Vector2Int(coordinates.x + initialCoordinates.x, coordinates.y + initialCoordinates.z);
        newCell.name = (coordinates.x + initialCoordinates.x) + "," + (coordinates.y + initialCoordinates.z);
        newCell.transform.position = new Vector3(initialCoordinates.x + (coordinates.x * MazeCell.cellSize), initialCoordinates.y, initialCoordinates.z + (coordinates.y * MazeCell.cellSize));

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
            int r = cell.coordinates.y - initialCoordinates.z;

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
