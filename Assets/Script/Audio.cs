using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip Clip;
    // Static singleton property.
    public static Audio Instance { get; private set; }
    void Awake()
    {
        // Save a reference to the AudioManager component as our //singleton instance.
        Instance = this;
    }
    // Instance method, this method can be accessed through the //singleton instance
    public void PlayAudio(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
