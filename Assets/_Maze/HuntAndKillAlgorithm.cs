using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HuntAndKillAlgorithm : MazeAlgorithm
{
    public HuntAndKillAlgorithm(MazeCell[,] cells) : base(cells)
    {
    }

    public override void CreateMaze()
    {
        MazeCell currentCell = GetRandomCell();

        while (currentCell)
        {
            List<MazeCell> unvisitedNeighbours = currentCell.Neighbors.Where(c => c.GetLinks().Count == 0).ToList();

            if (unvisitedNeighbours.Count > 0)
            {
                MazeCell neighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                currentCell.CreatePassage(neighbour);
                currentCell = neighbour;
            }
            else // Hunt
            {
                currentCell = null;

                foreach (var cell in cells)
                {
                    List<MazeCell> visitedNeighbours = cell.Neighbors.Where(c => c.GetLinks().Count > 0).ToList();
                    if (cell.GetLinks().Count == 0 && visitedNeighbours.Count > 0)
                    {
                        currentCell = cell;

                        MazeCell neighbour = visitedNeighbours[Random.Range(0, visitedNeighbours.Count)];
                        currentCell.CreatePassage(neighbour);

                        break;
                    }
                }
            }
        }
    }
}