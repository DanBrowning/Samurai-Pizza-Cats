using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    static SoundManager _instance = null;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    
    public AudioClip EDefeat;
    public AudioClip menuSong;
    public AudioClip levelSong;
    public AudioClip creditSong;
    public AudioClip gameOverSong;
    public AudioClip dieSound;
    public AudioClip hurtSound;
    
    // Use this for initialization
    void Start()
    {
        if (instance)
            DestroyImmediate(gameObject); // destroys the new Game_Manager upon scenes being loaded and keeps the old one
        else
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        SoundManager.instance.playESound(SoundManager.instance.menuSong);


    }

    // Update is called once per frame
    void Update()
    {

    }

    public static SoundManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public void playSingleSound(AudioClip clip, float volume = 1.0f)
    {
        // assign volume to AudioSource
        sfxSource.volume = volume;

        //assign clip to AudioSource clip
        sfxSource.clip = clip;

        //play assigned audioClip through AudioSource on SoundManager
        sfxSource.Play();
    }
    public void playESound(AudioClip clip, float volume = 1.0f)
    {
        // assign volume to AudioSource
        musicSource.volume = volume;

        //assign clip to AudioSource clip
        musicSource.clip = clip;

        //play assigned audioClip through AudioSource on character
        musicSource.Play();

    }





}
