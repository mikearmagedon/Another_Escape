using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{

    public AudioClip clip;

    private AudioManager audioManager;
    [HideInInspector] public string type;

    // Use this for initialization
    void Start()
    {
        audioManager = AudioManager.instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioManager.ChangeMusic(clip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name == "mainhub_trigger")
            {
                type = "stone";
            }
            else
            {
                type = "grass";

            }
        }
    }

    //private void OnTriggerExit()
    //{
    //    StartCoroutine(AM.FadeOut());
    //}
}
