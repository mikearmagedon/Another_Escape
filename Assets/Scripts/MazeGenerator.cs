using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int initialCoordinates;
    [SerializeField] Vector2Int size;
    [SerializeField] Player player;
    [SerializeField] Text messageText;
    [SerializeField] MazeCell cellPrefab;
    [SerializeField] float secondsBetweenGenerations = 5f;

    private MazeCell[,] cells;
    private MazeAlgorithm ma;

    // Use this for initialization
    void Start()
    {
        InitializeMaze();
        ConfigureCells();

        ma = new BinaryTreeAlgorithm(cells);
        StartCoroutine(GameLoop());
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
     * Assigns the neighbours of each cell
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

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartGame());
        yield return StartCoroutine(PlayGame());
        yield return StartCoroutine(EndGame());

        SceneManager.LoadScene(0);
    }

    IEnumerator StartGame()
    {
        player.DisableControl();
        yield return new WaitForSeconds(3f);
        messageText.text = "Find all the coins and return to the start!";
        yield return new WaitForSeconds(3f);
    }

    IEnumerator PlayGame()
    {
        player.EnableControl();

        messageText.text = string.Empty;

        StartCoroutine(MazeRegeneration());

        while (!player.wonGame)
        {
            yield return null;
        }
    }

    IEnumerator MazeRegeneration()
    {
        yield return StartCoroutine(ma.CreateMaze());
        yield return new WaitForSeconds(secondsBetweenGenerations);
        ResetMaze();
        StartCoroutine(MazeRegeneration());
    }

    IEnumerator EndGame()
    {
        player.DisableControl();

        messageText.text = "GAME OVER";

        yield return new WaitForSeconds(3f);
    }

    private void ResetMaze() // TODO consider moving this function to MazeAlgorithm
    {
        foreach (var cell in cells)
        {
            cell.Reset();
        }
    }
}
