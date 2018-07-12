using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScenePersist : MonoBehaviour
{
    int startingSceneIndex;
    public static ScenePersist current;

    void Awake()
    {
        // Enforce singleton pattern
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        int currentSceneIndex = scene.buildIndex;
        if (currentSceneIndex != startingSceneIndex)
        {
            Destroy(gameObject);
        }
    }
}
