﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    internal const int cellSize = 1;
    internal Vector2Int coordinates;
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
    }

    internal void Reset()
    {
        //links = new Dictionary<MazeCell, bool>();
        if (links.Count > 0)
        {
            foreach (var cell in links.Keys)
            {
                links[cell] = false;
            }
        }
    }

    internal void CreatePassage(MazeCell neighbour, bool bidirectional = true)
    {
        links[neighbour] = true;
        if (bidirectional)
        {
            neighbour.CreatePassage(this, false);
        }
    }
}
