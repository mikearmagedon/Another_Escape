using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // TODO consider using an array
    public bool northWallActive = true;
    public bool eastWallActive = true;
    public bool southWallActive = true;
    public bool westWallActive = true;

    internal const int cellSize = 1;
    internal Vector2Int coordinates;

    [SerializeField] GameObject northWall, eastWall, southWall, westWall;

    // Use this for initialization
    void Start()
    {
        northWall.SetActive(northWallActive);
        eastWall.SetActive(eastWallActive);
        southWall.SetActive(southWallActive);
        westWall.SetActive(westWallActive);
    }

    // Update is called once per frame
    void Update()
    {
        northWall.SetActive(northWallActive);
        eastWall.SetActive(eastWallActive);
        southWall.SetActive(southWallActive);
        westWall.SetActive(westWallActive);
    }

    internal void Reset()
    {
        northWallActive = true;
        eastWallActive = true;
        southWallActive = true;
        westWallActive = true;
    }

    internal void CreatePassage(MazeCell neighbour, int direction)
    {
        switch (direction)
        {
            case 0:
                northWallActive = false;
                // Set northern neighbour cell's southwall as passage
                neighbour.southWallActive = false;
                break;
            case 1:
                eastWallActive = false;
                // Set northern neighbour cell's southwall as passage
                neighbour.westWallActive = false;
                break;
            default:
                break;
        }
    }
}
