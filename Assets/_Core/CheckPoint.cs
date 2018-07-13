using UnityEngine;
using System.Collections;
using RPG.Characters;
using System;

public class CheckPoint : MonoBehaviour
{
    // have we been triggered?
    [HideInInspector] public bool triggered;
    [HideInInspector] public string nameCheckpoint;

    void Awake()
    {
        triggered = false;
        nameCheckpoint = gameObject.name;
    }

    // called whenever another collider enters our zone (if layers match)
    void OnTriggerEnter(Collider collider)
    {
        // check we haven't been triggered yet!
        if (!triggered)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                triggered = true;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
