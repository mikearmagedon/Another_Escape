using UnityEngine;

public class opendoor : MonoBehaviour {

    public GameObject boss;
    public GameObject portal;
    public AudioClip clip;

    private Animation anim;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if(boss == null)
        {
            anim = this.GetComponent<Animation>();
            anim.Play("door");
            audioManager.PlayMisc(clip);
            portal.SetActive(true);
            enabled = false;
        }
    }
}