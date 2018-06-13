using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		
	}

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }
}
