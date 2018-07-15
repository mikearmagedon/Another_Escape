using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioClip clip;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.ChangeMusic(clip);
    }

    //void Awake()
    //{
    //       int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
    //       if (numMusicPlayers > 1)
    //       {
    //           Destroy(gameObject);
    //       }
    //       else
    //       {
    //           DontDestroyOnLoad(gameObject);
    //       }
    //}
}
