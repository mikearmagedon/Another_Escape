using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrowingTreeAlgorithm : MazeAlgorithm
{
    public GrowingTreeAlgorithm(MazeCell[,] cells) : base(cells)
    {
    }

    public override void CreateMaze()
    {
        List<MazeCell> activeCells = new List<MazeCell>();
        activeCells.Add(GetRandomCell());

        while (activeCells.Count > 0)
        {
            MazeCell currentCell = activeCells[activeCells.Count - 1];
            List<MazeCell> availableNeighbours = currentCell.Neighbors.Where(c => c.GetLinks().Count == 0).ToList();

            if (availableNeighbours.Count > 0)
            {
                MazeCell neighbour = availableNeighbours[Random.Range(0, availableNeighbours.Count)];
                currentCell.CreatePassage(neighbour);
                activeCells.Add(neighbour);
            }
            else
            {
                activeCells.Remove(currentCell);
            }
        }
    }
}
