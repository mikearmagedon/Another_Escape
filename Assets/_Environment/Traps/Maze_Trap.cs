using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Trap : MonoBehaviour {

    public AudioClip clip;
    AudioManager audioManager;

	// Use this for initialization
	void Start () {
        audioManager = AudioManager.instance;
        InvokeRepeating("PlayAnimation", 0f,2f);
	}

    void PlayAnimation()
    {
        GetComponent<Animation>()["Maze_Trap"].speed = 2f;
        GetComponent<Animation>().Play();
        audioManager.PlayMisc(clip);
    }
}
