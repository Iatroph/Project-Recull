using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Effect", menuName = "Sound Effect", order = 1)]
public class SoundFX : ScriptableObject
{
    public AudioClip soundEffect;
    public string soundName;
    [Range(0f, 1f)]
    public float volume = 1;
}
