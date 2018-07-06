using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_sound : MonoBehaviour
{

    public AudioSource music;
    private AudioSource musicmisc;

    private void Start()
    {
        musicmisc = gameObject.AddComponent<AudioSource>();
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
        musicmisc.clip = clip;
        musicmisc.spatialBlend = 0.5f;
        musicmisc.loop = false;
        musicmisc.volume = 0.6f;
        musicmisc.PlayOneShot(clip);
    }

    public IEnumerator FadeOut()
    {
        float t = 0.3f;
        while (t >= 0.0f)
        {
            t -= Time.deltaTime;
            music.volume = t;
            yield return new WaitForSeconds(0);
        }
        music.volume = 0.0f;
        music.Stop();
        music.loop = false;
    }

    IEnumerator FadeIn()
    {
        music.volume = 0.0f;
        music.Play();
        music.loop = true;

        float t = 0.0f;
        while (music.volume <= 0.3f)
        {
            t += Time.deltaTime;
            music.volume = t;
            yield return new WaitForSeconds(0);
        }
    }
}
