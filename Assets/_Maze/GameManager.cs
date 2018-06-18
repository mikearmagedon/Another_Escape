using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPG.Characters; // to access PlayerController

public class GameManager : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] Image levelTransition;
    [SerializeField] float levelStartDelay = 3f;
    [SerializeField] PlayerController player;

    int currentSceneIndex;

    // Use this for initialization
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(GameLoop());
	}

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartGame());
        yield return StartCoroutine(PlayGame());
        yield return StartCoroutine(EndGame());
    }

    IEnumerator StartGame()
    {
        player.DisableControl();
        messageText.text = "Level " + currentSceneIndex;
        levelTransition.gameObject.SetActive(true);
        yield return new WaitForSeconds(levelStartDelay);
        levelTransition.gameObject.SetActive(false);
        messageText.text = string.Empty;
        messageText.enabled = false;
    }

    IEnumerator PlayGame()
    {
        player.EnableControl();

        while (!player.wonGame)
        {
            yield return null;
        }
    }
    
    IEnumerator EndGame()
    {
        messageText.enabled = true;
        messageText.text = "GAME OVER";

        yield return new WaitForSeconds(3f);

        // player died
        if (!player.enabled)
        {
            SceneManager.LoadScene(currentSceneIndex);
        }
        else // player finished level
        {
            if ((currentSceneIndex + 1) < SceneManager.sceneCountInBuildSettings)
            {
                // load next level
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                // load main menu
                SceneManager.LoadScene(0);
            }
        }
    }
}
