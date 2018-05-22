using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    internal const int cellSize = 4;
    internal Vector2Int coordinates;
    internal MazeDirection entrance = MazeDirection.None;
    internal MazeDirection exit = MazeDirection.None;
    internal MazeCell North { get; set; }
    internal MazeCell South { get; set; }
    internal MazeCell East { get; set; }
    internal MazeCell West { get; set; }
    internal List<MazeCell> Neighbors
    {
        get
        {
            var list = new List<MazeCell>();
            if (North != null) list.Add(North);
            if (East != null) list.Add(East);
            if (South != null) list.Add(South);
            if (West != null) list.Add(West);
            return list;
        }
    }

    [SerializeField] GameObject[] walls;
    [SerializeField] Dictionary<MazeCell, bool> links;

    void Awake()
    {
        links = new Dictionary<MazeCell, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        // Draw passages with neighbours
        if (links.Count == 0) { return; }

        if (North && links.ContainsKey(North))
        {
            walls[(int)MazeDirection.North].SetActive(false);
        }
        else
        {
            walls[(int)MazeDirection.North].SetActive(true);
        }
        if (East && links.ContainsKey(East))
        {
            walls[(int)MazeDirection.East].SetActive(false);
        }
        else
        {
            walls[(int)MazeDirection.East].SetActive(true);
        }
        if (South && links.ContainsKey(South))
        {
            walls[(int)MazeDirection.South].SetActive(false);
        }
        else
        {
            walls[(int)MazeDirection.South].SetActive(true);
        }
        if (West && links.ContainsKey(West))
        {
            walls[(int)MazeDirection.West].SetActive(false);
        }
        else
        {
            walls[(int)MazeDirection.West].SetActive(true);
        }

        // Draw exit/entrance
        foreach (MazeDirection direction in Enum.GetValues(typeof(MazeDirection)))
        {
            if (entrance == MazeDirection.None && exit == MazeDirection.None) { return; }
            if (direction == MazeDirection.None) { continue; }

            if (direction == entrance)
            {
                walls[(int)direction].SetActive(false);
            }

            if (direction == exit)
            {
                walls[(int)direction].SetActive(false);
            }         
        }
    }

    internal void Reset()
    {
        links.Clear();
    }

    internal void CreatePassage(MazeCell neighbour, bool bidirectional = true)
    {
        links[neighbour] = true;
        if (bidirectional)
        {
            neighbour.CreatePassage(this, false);
        }
    }

    internal void DestroyWall(MazeDirection direction)
    {
        walls[(int)direction].SetActive(false);
    }
}
