using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] songs;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            MyAudioManager.instance.PlayMusic(songs[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
