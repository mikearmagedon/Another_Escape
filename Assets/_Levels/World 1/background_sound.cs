using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_sound : MonoBehaviour
{

    public AudioSource music;

    public void ChangeMusic(AudioClip clip)
    {
        if (music.clip.name == clip.name)
        {
            return;
        }

        music.clip = clip;
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float t = 0.5f;
        while (t >= 0)
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
        music.loop = false;

        float t = 0.0f;
        while (music.volume <= 0.5)
        {
            t += Time.deltaTime;
            music.volume = t;
            yield return new WaitForSeconds(0);
        }

    }
}
