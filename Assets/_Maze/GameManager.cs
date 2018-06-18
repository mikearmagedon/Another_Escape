using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPG.Characters; // to access PlayerController

public class GameManager : MonoBehaviour
{
    // Config
    [SerializeField] float levelStartDelay = 3f;

    // State
    bool isPaused = false;
    int currentSceneIndex;
    float initialFixedDelta;

    // Cached components references
    Text messageText;
    GameObject levelTransition;
    PlayerController player;

    void Awake()
    {
        // Enforce singleton pattern
        int numGameManager = FindObjectsOfType<GameManager>().Length;
        if (numGameManager > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        initialFixedDelta = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void OnApplicationPause(bool pause)
    {
        isPaused = pause;
        PauseGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;
        player = FindObjectOfType<PlayerController>();
        levelTransition = GameObject.Find("Level Transition");
        messageText = GameObject.Find("Text").GetComponent<Text>();

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
        levelTransition.SetActive(true);
        yield return new WaitForSeconds(levelStartDelay);
        levelTransition.SetActive(false);
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

    void PauseGame()
    {
        if (isPaused)
        {
            // TODO enable pause menu
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0;
        }
        else
        {
            // TODO disable pause menu
            Time.timeScale = 1f;
            Time.fixedDeltaTime = initialFixedDelta;
        }
        isPaused = !isPaused;
    }
}
