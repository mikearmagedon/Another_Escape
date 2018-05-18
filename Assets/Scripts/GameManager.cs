using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] Player player;
    [SerializeField] GameObject mazeGeneratorPrefab;
    [SerializeField] MazeGeneratorManager[] mazeGenerators;

    // Use this for initialization
    void Start()
    {
        InitializeMazeGenerators();
        StartCoroutine(GameLoop());
	}

    void InitializeMazeGenerators()
    {
        for (int i = 0; i < mazeGenerators.Length; i++)
        {
            mazeGenerators[i].mazeGeneratorInstance = Instantiate(mazeGeneratorPrefab, transform);
            mazeGenerators[i].Setup();
        }
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartGame());
        yield return StartCoroutine(PlayGame());
        yield return StartCoroutine(EndGame());

        SceneManager.LoadScene(0);
    }

    IEnumerator StartGame()
    {
        player.DisableControl();
        yield return new WaitForSeconds(3f);
        messageText.text = "Find all the coins and return to the start!";
        yield return new WaitForSeconds(3f);
    }

    IEnumerator PlayGame()
    {
        player.EnableControl();

        messageText.text = string.Empty;

        // TODO foreach mazeGenerator in mazeGenerators
        //          call StartCoroutine(mazeGenerator.ContinuousMazeGeneration());
        //StartCoroutine(MazeRegeneration());

        //foreach (var mazeGenerator in mazeGenerators)
        //{
        //    StartCoroutine(mazeGenerator.mazeGenerator.ContinuousMazeGeneration());
        //}

        while (!player.wonGame)
        {
            yield return null;
        }
    }
    
    IEnumerator EndGame()
    {
        player.DisableControl();

        messageText.text = "GAME OVER";

        yield return new WaitForSeconds(3f);
    }
}
