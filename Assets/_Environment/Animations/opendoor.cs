using UnityEngine;

public class opendoor : MonoBehaviour {

    public GameObject Door;
    public GameObject portal;

    private Animation anim;

    void OnTriggerEnter()
    {
        anim = Door.GetComponent<Animation>();
        anim.Play("door");
        this.gameObject.GetComponent<Collider>().enabled = false;
        portal.SetActive(true);
    }

}