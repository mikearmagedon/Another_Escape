using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeAlgorithm : MazeAlgorithm
{
    public BinaryTreeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
    {

    }

    public override IEnumerator CreateMaze()
    {
        foreach (var cell in cells)
        {
            List<MazeCell> neighbours = new List<MazeCell>();
            if (cell.North != null)
            {
                neighbours.Add(cell.North);
            }
            if (cell.East != null)
            {
                neighbours.Add(cell.East);
            }

            int index = Random.Range(0, neighbours.Count);

            if (index < neighbours.Count)
            {
                MazeCell neighbour = neighbours[index];
                yield return new WaitForSeconds(0.1f);
                cell.CreatePassage(neighbour);
            }
        }
    }
}
