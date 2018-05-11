using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // TODO consider using an array
    [SerializeField] bool northWallActive = true;
    [SerializeField] bool eastWallActive = true;
    [SerializeField] bool southWallActive = true;
    [SerializeField] bool westWallActive = true;

    public GameObject northWall, eastWall, southWall, westWall;

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
