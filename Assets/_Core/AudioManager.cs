using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource music;
    private AudioSource musicMisc;
    [HideInInspector] public AudioSource musicBattle;

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

    void Start()
    {
        musicMisc = gameObject.AddComponent<AudioSource>();
        musicMisc.playOnAwake = false;
        musicBattle = gameObject.AddComponent<AudioSource>();
        musicBattle.playOnAwake = false;
    }

    public void PlayMusicBattle(AudioClip clip, bool inBattle)
    {
        if (inBattle)
        {
            if (musicBattle.isPlaying)
            {
                return;
            }
            else
            {
                musicBattle.clip = clip;
                StartCoroutine(FadeIn(musicBattle));
                music.Pause();
            }
        }
        else
        {
            StartCoroutine(FadeOut(musicBattle));
            music.UnPause();
        }

    }

    public void ChangeMusic(AudioClip clip)
    {
        if (music.clip.name == clip.name)
        {
            return;
        }

        music.clip = clip;
        StartCoroutine(FadeIn(music));
    }

    public void PlayMisc(AudioClip clip)
    {
        //if (musicmisc.isPlaying)
        //{
        //    return;
        //}
        musicMisc.clip = clip;
        musicMisc.spatialBlend = 0.5f;
        musicMisc.loop = false;
        musicMisc.volume = 0.6f;
        musicMisc.PlayOneShot(clip);
    }

    public IEnumerator FadeOut(AudioSource source)
    {
        float t = 0.2f;
        while (t >= 0.0f)
        {
            t -= Time.deltaTime;
            source.volume = t;
            yield return new WaitForSeconds(0);
        }
        source.volume = 0.0f;
        source.Stop();
        source.loop = false;
    }

    IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0.0f;
        source.Play();
        source.loop = true;

        float t = 0.0f;
        while (source.volume <= 0.2f)
        {
            t += Time.deltaTime;
            source.volume = t;
            yield return new WaitForSeconds(0);
        }
    }
}
