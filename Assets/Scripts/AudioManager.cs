﻿using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup musicMixerGroup;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if(s.name == "BackGroundMusic")
            {              
                s.source.outputAudioMixerGroup = musicMixerGroup;
            }
            else
            {
                s.source.outputAudioMixerGroup = audioMixerGroup;
            }
            

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        Play("BackGroundMusic");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        else
        {
            s.source.Play();
        }


      
    }
}
