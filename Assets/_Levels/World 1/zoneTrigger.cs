using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneTrigger : MonoBehaviour {

    public AudioClip clip;

    private background_sound AM;

	// Use this for initialization
	void Start () {
        AM = FindObjectOfType<background_sound>();
	}

    private void OnTriggerEnter()
    {
        AM.ChangeMusic(clip);
    }

    private void OnTriggerExit()
    {
        StartCoroutine(AM.FadeOut());
    }
}
