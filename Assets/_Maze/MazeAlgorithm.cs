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

    public abstract void CreateMaze();
}
