using UnityEngine;
using System.Collections;

public class BinaryTreeAlgorithm : MazeAlgorithm
{
	private int currentRow = 0;
	private int currentColumn = 0;

	private bool courseComplete = false;

	public BinaryTreeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
    {

	}

	public override void CreateMaze()
    {
		BinaryTree();
	}

	private void BinaryTree()
    {
        foreach (var cell in cells)
        {
            int direction = Random.Range(0, 1);
            if (direction == 1)
            {
                cell.southWallActive = false;
            }
            else
            {
                cell.eastWallActive = false;
            }
        }
	}
}
