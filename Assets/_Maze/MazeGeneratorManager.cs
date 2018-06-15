using System;
using UnityEngine;

[Serializable]
public class MazeGeneratorManager
{
    [HideInInspector] public GameObject mazeGeneratorInstance;
    [SerializeField] Vector3Int initialCoordinates = new Vector3Int(0, 0, 0);
    [SerializeField] Vector2Int size = new Vector2Int(10, 10);
    [SerializeField] Algorithm mazeAlgorithm = Algorithm.BinaryTree;
    [SerializeField] float secondsBetweenGenerations = 20f;
    [SerializeField] Vector2Int mazeEntrance = new Vector2Int(0, 0);
    [SerializeField] MazeDirection mazeEntranceDirection = MazeDirection.None;
    [SerializeField] Vector2Int mazeExit = new Vector2Int(0, 0);
    [SerializeField] MazeDirection mazeExitDirection = MazeDirection.None;

    MazeGenerator mazeGenerator;

    public void Setup()
	{
        mazeGenerator = mazeGeneratorInstance.GetComponent<MazeGenerator>();

        mazeGenerator.size = size;
        mazeGenerator.initialCoordinates = initialCoordinates;
        mazeGenerator.mazeAlgorithm = mazeAlgorithm;
        mazeGenerator.secondsBetweenGenerations = secondsBetweenGenerations;
        mazeGenerator.mazeEntrance = mazeEntrance;
        mazeGenerator.mazeEntranceDirection = mazeEntranceDirection;
        mazeGenerator.mazeExit = mazeExit;
        mazeGenerator.mazeExitDirection = mazeExitDirection;
    }
}
