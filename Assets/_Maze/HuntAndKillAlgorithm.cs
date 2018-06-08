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
        MazeCell current = GetRandomCell();

        while (current)
        {
            List<MazeCell> unvisitedNeighbours = current.Neighbors.Where(c => c.GetLinks().Count == 0).ToList();

            if (unvisitedNeighbours.Count != 0)
            {
                MazeCell neighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                current.CreatePassage(neighbour);
                current = neighbour;
            }
            else // Hunt
            {
                current = null;

                foreach (var cell in cells)
                {
                    List<MazeCell> visitedNeighbours = cell.Neighbors.Where(c => c.GetLinks().Count != 0).ToList();
                    if (cell.GetLinks().Count == 0 && visitedNeighbours.Count != 0)
                    {
                        current = cell;

                        MazeCell neighbour = visitedNeighbours[Random.Range(0, visitedNeighbours.Count)];
                        current.CreatePassage(neighbour);

                        break;
                    }
                }
            }
        }
    }
}