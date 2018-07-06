using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour {

    public AudioClip clip;

    private Background_sound AM;

	// Use this for initialization
	void Start () {
        AM = FindObjectOfType<Background_sound>();
	}

    private void OnTriggerStay()
    {
        AM.ChangeMusic(clip);
    }

    //private void OnTriggerExit()
    //{
    //    StartCoroutine(AM.FadeOut());
    //}
}
