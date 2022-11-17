using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public enum AudioChannel { Master = 0, SFX = 1, Music = 2 };

    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    private AudioSource OneShotSource;

    private void Awake()
    {
        OneShotSource = GetComponent<AudioSource>();

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            OneShotSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
    }

    public void PlaySoundAtPoint(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolume * masterVolume);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
