using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOpen : MonoBehaviour {

    public GameObject TrapDoor;
    public GameObject FX_Dust;

    private Animation anim;

    void OnTriggerEnter()
    {
        anim = TrapDoor.GetComponent<Animation>();
        foreach (AnimationState state in anim)
        {
            state.speed = 2F;
        }
        anim.Play("TrapAnimRock");
        TrapDoor.GetComponent<Collider>().enabled = false;
        TrapDoor.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        FX_Dust.SetActive(true);
    }

}
