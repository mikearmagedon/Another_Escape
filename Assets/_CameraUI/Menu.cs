using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class Menu : MonoBehaviour
{

    public void LoadFirstLevel()
    {
        Assert.IsTrue(SceneManager.sceneCountInBuildSettings > 1, "Please add more than one scene to the build settings");
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        GameManager.instance.PauseGame(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(FindObjectOfType<SaveLoad>().LoadMenu());
        FindObjectOfType<SaveLoad>().loadGame = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
