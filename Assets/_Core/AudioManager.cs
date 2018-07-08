using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource music;
    private AudioSource musicmisc;

    void Awake()
    {
        int numAudioManager = FindObjectsOfType<AudioManager>().Length;
        if (numAudioManager > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        musicmisc = gameObject.AddComponent<AudioSource>();
        musicmisc.playOnAwake = false;
    }

    public void ChangeMusic(AudioClip clip)
    {
        if (music.clip.name == clip.name)
        {
            return;
        }

        music.clip = clip;
        StartCoroutine(FadeIn());
    }

    public void PlayMisc(AudioClip clip)
    {
        //if (musicmisc.isPlaying)
        //{
        //    return;
        //}
        musicmisc.clip = clip;
        musicmisc.spatialBlend = 0.5f;
        musicmisc.loop = false;
        musicmisc.volume = 0.6f;
        musicmisc.PlayOneShot(clip);
    }

    //public IEnumerator FadeOut()
    //{
    //    float t = 0.3f;
    //    while (t >= 0.0f)
    //    {
    //        t -= Time.deltaTime;
    //        music.volume = t;
    //        yield return new WaitForSeconds(0);
    //    }
    //    music.volume = 0.0f;
    //    music.Stop();
    //    music.loop = false;
    //}

    IEnumerator FadeIn()
    {
        music.volume = 0.0f;
        music.Play();
        music.loop = true;

        float t = 0.0f;
        while (music.volume <= 0.2f)
        {
            t += Time.deltaTime;
            music.volume = t;
            yield return new WaitForSeconds(0);
        }
    }
}
