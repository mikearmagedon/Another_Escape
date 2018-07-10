using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSoundController : MonoBehaviour
{

    List<AudioSource> waterSounds;
    GameObject player;

    // Use this for initialization
    void Start ()
    {
        waterSounds = new List<AudioSource>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Water"))
        {
            waterSounds.Add(obj.GetComponent<AudioSource>());
        }
        player = GameObject.Find("Player");
    }

	// Update is called once per frame
	void Update ()
    {
        DetermineClosestWaterSound();
    }

    void DetermineClosestWaterSound()
    {
        float distance = 10f;
        //Determine which water sound is the closest
        foreach (AudioSource waterSound in waterSounds)
        {
            float newDistance = Vector3.Distance(player.transform.position, waterSound.transform.position);
            if (newDistance < distance)
            {
                waterSound.enabled = true;
            }
            else
            {
                waterSound.enabled = false;
            }
        }
    }
}
