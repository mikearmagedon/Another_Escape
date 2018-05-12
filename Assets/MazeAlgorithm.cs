// From https://www.youtube.com/watch?v=IrO4mswO2o4

using UnityEngine;
using System.Collections;

public abstract class MazeAlgorithm
{
	protected MazeCell[,] cells;
	protected int mazeRows, mazeColumns;

	protected MazeAlgorithm(MazeCell[,] cells) : base()
    {
		this.cells = cells;
		mazeRows = cells.GetLength(0);
		mazeColumns = cells.GetLength(1);
	}

	public abstract IEnumerator CreateMaze();

    protected MazeCell GetCell(Vector2Int coordinates)
    {
        return cells[coordinates.x, coordinates.y];
    }

    protected bool ContainsCoordinates(Vector2Int coordinates)
    {
        return (0 <= coordinates.x && coordinates.x < mazeRows && 0 <= coordinates.y && coordinates.y < mazeColumns);
    }
}
