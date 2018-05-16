using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MazeGeneratorManager
{
    [HideInInspector] public GameObject mazeGeneratorInstance;
    [SerializeField] Vector2Int initialCoordinates = new Vector2Int(0, 0);
    [SerializeField] Vector2Int size = new Vector2Int(10, 10);
    [SerializeField] float secondsBetweenGenerations = 20f;

    [HideInInspector] public MazeGenerator mazeGenerator; // TODO make this variable private

    public void Setup()
	{
        mazeGenerator = mazeGeneratorInstance.GetComponent<MazeGenerator>();

        mazeGenerator.size = size;
        mazeGenerator.initialCoordinates = initialCoordinates;
        mazeGenerator.secondsBetweenGenerations = secondsBetweenGenerations;
    }
}
