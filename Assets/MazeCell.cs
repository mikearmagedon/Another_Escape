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
}
