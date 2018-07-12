using UnityEngine;
using System.Collections;
using RPG.Characters;
using System;

public class CheckPoint : MonoBehaviour
{
    // have we been triggered?
    bool triggered;

    void Awake()
    {
        triggered = false;
    }

    // called whenever another collider enters our zone (if layers match)
    void OnTriggerEnter(Collider collider)
    {
        // check we haven't been triggered yet!
        if (!triggered)
        {
            // check we actually collided with 
            // a character. It would be best to
            // setup your layers so this check is
            // not required, by creating a layer 
            // "Checkpoint" that will only collide 
            // with characters.
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                
            }
        }
    }

}
