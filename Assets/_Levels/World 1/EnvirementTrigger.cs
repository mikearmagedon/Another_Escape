using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvirementTrigger : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioClip clip;

    private Background_sound AM;

    // Use this for initialization
    void Start()
    {
        AM = FindObjectOfType<Background_sound>();
    }

    private void OnTriggerEnter()
    {
        InvokeRepeating("Call",0,8f);
    }

    private void OnTriggerExit()
    {
        CancelInvoke();
    }

    private void Call()
    {
        clip = clips[Random.Range(0, clips.Length)];
        AM.PlayMisc(clip);
    }
}
