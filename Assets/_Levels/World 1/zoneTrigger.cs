using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{

    public AudioClip clip;

    private Background_sound AM;
    //AudioClip[] soundgrass;
    //AudioClip[] soundstone;
    public string type;

    // Use this for initialization
    void Start()
    {
        AM = FindObjectOfType<Background_sound>();
    }

    private void OnTriggerStay()
    {
        AM.ChangeMusic(clip);

    }

    private void OnTriggerEnter()
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

    //public void GetFootSetpSounds(AudioClip[] grass, AudioClip[] stone)
    //{
    //    soundgrass = grass;
    //    soundstone = stone;
    //}

    //public AudioClip SetFootSetSounds()
    //{
    //    if (type == "stone")
    //    {
    //        AudioClip stone = soundstone[Random.Range(0, soundstone.Length)];
    //        return stone;
    //    }
    //    else if (type == "grass")
    //    {
    //        AudioClip grass = soundgrass[Random.Range(0, soundgrass.Length)];
    //        return grass;
    //    }
    //    return null;
    //}

    //private void OnTriggerExit()
    //{
    //    StartCoroutine(AM.FadeOut());
    //}
}
