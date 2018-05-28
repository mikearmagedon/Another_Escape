using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewinderAlgorithm : MazeAlgorithm
{
    public SidewinderAlgorithm(MazeCell[,] cells) : base(cells)
    {
    }

    public override void CreateMaze()
    {
        for (int r = 0; r < mazeRows; r++)
        {
            List<MazeCell> run = new List<MazeCell>();
            
            for (int c = 0; c < mazeColumns; c++)
            {
                MazeCell currentCell = cells[c, r];
                run.Add(currentCell);

                bool isAtEasternBoundary = (currentCell.East == null);
                bool isAtNorthernBoundary = (currentCell.North == null);

                bool shouldCloseOut = isAtEasternBoundary || (!isAtNorthernBoundary && Random.Range(0, 2) == 0);

                if (shouldCloseOut)
                {
                    MazeCell member = run[Random.Range(0, run.Count)];
                    if (member.North != null)
                    {
                        member.CreatePassage(member.North);
                    }
                    run.Clear();
                }
                else
                {
                    currentCell.CreatePassage(currentCell.East);
                }
            }
        }
    }
}
