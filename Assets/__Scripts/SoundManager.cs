using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    public AudioClip[] soundtracks;
    public AudioSource soundtrackSource;
    public AudioSource buttonClickSource;

    [Header("Set Dynamically")]
    public static bool soundIsMuted = false;
    public bool musicStopped = false;
    public int soundtrackNum = 0;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (soundtrackSource == null)
            {
                soundtrackSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
            }
            if (!soundtrackSource.isPlaying)
            {
                soundtrackSource.clip = soundtracks[1];
                soundtrackSource.Play();
            }
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!musicStopped)
            {
                if (soundtrackSource.clip.name == "mainMenuTheme")
                {
                    soundtrackSource.Stop();
                }
                if (Hero.S != null)
                {
                    if (!soundtrackSource.isPlaying || soundtrackSource.clip.name == "endGameSound")
                    {
                        RandomSoudntrack();
                    }
                    if (Input.GetKeyDown(KeyCode.N))
                    {
                        NextSong();
                    }
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        PrevSong();
                    }
                }
                else
                {
                    if (soundtrackSource.clip.name != "endGameSound")
                    {
                        soundtrackSource.Stop();
                    }
                    if (!soundtrackSource.isPlaying)
                    {
                        soundtrackSource.clip = soundtracks[0];
                        soundtrackSource.Play();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                musicStopped = false;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                musicStopped = true;
                soundtrackSource.Stop();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Mute();

        }
    }
    public void RandomSoudntrack()
    {
        soundtrackNum = Random.Range(2, soundtracks.Length);
        soundtrackSource.clip = soundtracks[soundtrackNum];
        soundtrackSource.Play();
    }
    public void MenuButtonsSound()
    {
        buttonClickSource = GameObject.Find("StatsPanel").GetComponent<AudioSource>();
        buttonClickSource.clip = Resources.Load<AudioClip>("Sound/metalButtonPressSound");
        buttonClickSource.Play();
    }
    public void Mute()
    {
        soundIsMuted = !soundIsMuted;
        if (soundIsMuted)
        {
            soundtrackSource.mute = true;
        }
        else
        {
            soundtrackSource.mute = false;
        }
    }
    public void NextSong()
    {
        if (soundtracks.Length > soundtrackNum + 1)
        {
            soundtrackNum++;
        }
        else
        {
            soundtrackNum = 2;
        }
        soundtrackSource.clip = soundtracks[soundtrackNum];
        soundtrackSource.Play();
    }
    public void PrevSong()
    {
        soundtrackNum--;
        if (soundtrackNum <= 1)
        {
            soundtrackNum = soundtracks.Length - 1;
        }
        soundtrackSource.clip = soundtracks[soundtrackNum];
        soundtrackSource.Play();
    }
}
