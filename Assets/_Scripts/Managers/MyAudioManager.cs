using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    public static MyAudioManager instance;
    public enum AudioChannel { Master = 0, SFX = 1, Music = 2 };

    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    private AudioSource OneShotSource;
    private AudioSource MusicSource;
    private AudioSource AmbienceSource;

    private void Awake()
    {
        OneShotSource = GetComponent<AudioSource>();

        //if (instance != null)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
            instance = this;
            DontDestroyOnLoad(gameObject);
            GameObject newMusicSource = new GameObject("Music Source");
            MusicSource = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
            MusicSource.volume = masterVolume * musicVolume;
        MusicSource.loop = true;

            GameObject newAmbienceSource = new GameObject("Ambience Source");
            AmbienceSource = newAmbienceSource.AddComponent<AudioSource>();
            newAmbienceSource.transform.parent = transform;
            AmbienceSource.volume = masterVolume * sfxVolume;
        AmbienceSource.loop = true;

        //}
    }

    public float GetSFXVolume()
    {
        return sfxVolume * masterVolume;
    }

    public void PlaySoundOneShot(SoundFX sound)
    {
        if (sound != null)
        {
            OneShotSource.PlayOneShot(sound.soundEffect, sfxVolume * masterVolume * sound.volume);
        }
    }

    public void PlaySoundAtPoint(SoundFX sound, Vector3 pos)
    {
        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound.soundEffect, pos, sfxVolume * masterVolume * sound.volume);
        }
    }

    public void PlayMusic(AudioClip song)
    {
        MusicSource.clip = song;
        MusicSource.Play();
    }

}
