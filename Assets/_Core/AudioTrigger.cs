using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float triggerRadius = 5f;
    [SerializeField] bool isOneTimeOnly = true;
    [SerializeField] bool isRepeatable = false;

    bool hasPlayed = false;
    //AudioSource audioSource;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.playOnAwake = false;
        //audioSource.clip = clips[Random.Range(0, clips.Length)];


        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            if (isRepeatable)
            {
                InvokeRepeating("PlayMusic", 0, 8f);
            }
            else
            {
                RequestPlayAudioClip();
            }
        }
    }

    void RequestPlayAudioClip()
    {
        if (isOneTimeOnly && hasPlayed)
        {
            return;
        }
        else
        {
            audioManager.PlayMisc(clips[Random.Range(0, clips.Length)]);
            hasPlayed = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }

    private void OnTriggerExit()
    {
        CancelInvoke();
    }

    private void PlayMusic()
    {
        audioManager.PlayMisc(clips[Random.Range(0, clips.Length)]);
    }
}
