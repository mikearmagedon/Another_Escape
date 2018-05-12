using UnityEngine;
using System.Collections;

public class BinaryTreeAlgorithm : MazeAlgorithm
{
	public BinaryTreeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
    {

	}

	public override IEnumerator CreateMaze()
    {
        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                int direction = Random.Range(0, 2);
                Vector2Int neighbour;
                if (direction == 0)
                {
                    neighbour = new Vector2Int(c, r + 1);
                }
                else
                {
                    neighbour = new Vector2Int(c + 1, r);
                }

                if (ContainsCoordinates(neighbour))
                {
                    yield return new WaitForSeconds(0.1f);
                    cells[c, r].CreatePassage(GetCell(neighbour), direction);
                }

            }
        }
    }    
}
