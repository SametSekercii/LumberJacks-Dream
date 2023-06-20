using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    public Slider musicSlider, sfxSlider;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

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

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, sfx2Source;

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, n => n.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            sfxSource.clip = sound.clip;
            sfxSource.Play();
        }

    }
    public void PlaySFX2(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            sfx2Source.clip = sound.clip;
            sfx2Source.Play();
        }

    }

    public void ChangeMusicValue()
    {
        musicSource.volume = musicSlider.value;
    }
    public void ChangeSfxValue()
    {
        sfxSource.volume = sfxSlider.value;
    }


}
