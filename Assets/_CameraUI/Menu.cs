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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
