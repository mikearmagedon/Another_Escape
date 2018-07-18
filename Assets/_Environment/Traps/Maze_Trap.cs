using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Trap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("PlayAnimation", 0f,2f);
	}

    void PlayAnimation()
    {
        GetComponent<Animation>()["Maze_Trap"].speed = 2f;
        GetComponent<Animation>().Play();
    }
}
