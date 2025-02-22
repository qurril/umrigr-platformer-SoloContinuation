using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public float musicVolume = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // PlayMusic("JDSherbert - Minigame Music Pack - Streetlights");
        musicSource.volume = musicVolume;
        musicSource.clip = musicSounds[0];
        musicSource.Play();
    }

    private void StopMusic()
    {
        musicSource.Stop();
    }

    public void AdjustVolume(float volume) {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void PlayMusic(string name)
    {
        AudioClip s = Array.Find(musicSounds, x => x.name == name);
        AdjustVolume(musicVolume);
        if (s == null)
        {
            Debug.Log("Sound Not Found: " + name);
        }
        else
        {
            musicSource.clip = s;
            musicSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        AudioClip s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found: " + name);
        }
        else
        {
            sfxSource.PlayOneShot(s);
        }
    }
}