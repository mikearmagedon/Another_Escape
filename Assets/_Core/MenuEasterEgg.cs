using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEasterEgg : MonoBehaviour {

    public float waitTime;
    float timer;
    GameObject player;
    GameObject pickups;

    void Start()
    {
        player = GameObject.Find("Player");
        pickups = GameObject.Find("Pickups");
        timer = 0;
        player.SetActive(false);
        pickups.SetActive(false);
    }

    void Update()
    {
        ActivatePlayer();
        PlayerInput();
        timer += Time.deltaTime;
    }

    void ActivatePlayer()
    {
        if (timer > waitTime)
        {
            player.SetActive(true);
            pickups.SetActive(true);
        }
    }

    void PlayerInput()
    {
        if (Input.anyKeyDown)
        {
            timer = 0;
        }
    }
}
