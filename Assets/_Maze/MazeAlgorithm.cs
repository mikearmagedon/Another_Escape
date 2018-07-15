// From https://www.youtube.com/watch?v=IrO4mswO2o4

using UnityEngine;

public abstract class MazeAlgorithm
{
    protected MazeCell[,] cells;
    protected int mazeRows, mazeColumns;

    protected MazeAlgorithm(MazeCell[,] cells) : base()
    {
        this.cells = cells;
        mazeColumns = cells.GetLength(0);
        mazeRows = cells.GetLength(1);
    }

    protected MazeCell GetRandomCell()
    {
        return cells[Random.Range(0, mazeColumns), Random.Range(0, mazeRows)];
    }

    public abstract void CreateMaze();
}
