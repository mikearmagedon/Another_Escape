using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource music;
    AudioSource musicMisc;
    AudioSource musicBattle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicMisc = gameObject.AddComponent<AudioSource>();
        musicMisc.playOnAwake = false;
        musicBattle = gameObject.AddComponent<AudioSource>();
        musicBattle.playOnAwake = false;
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            music.Pause();
            musicBattle.Pause();
            musicMisc.Pause();
        }
        else
        {
            music.UnPause();
            musicBattle.UnPause();
            musicMisc.UnPause();
        }
    }

    public void PlayMusicBattle(AudioClip clip, bool inBattle)
    {
        musicBattle.loop = true;
        musicBattle.volume = 0.3f;

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
        musicMisc.spatialBlend = 1f;
        musicMisc.loop = false;
        musicMisc.volume = 0.6f;
        musicMisc.maxDistance = 5f;
        musicMisc.PlayOneShot(clip);
    }

    public IEnumerator FadeOut(AudioSource source)
    {
        float t = 0.3f;
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