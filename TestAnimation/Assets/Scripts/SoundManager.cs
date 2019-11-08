using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    struct SoundInfo
    {
        public string key;
        public AudioClip value;
        public bool isMusic;
    }

    public static SoundManager instance;
    AudioSource musicAudioSource;
    AudioSource soundAudioSource;
    [SerializeField]
    List<SoundInfo> sounds;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        musicAudioSource = audioSources[0];
        soundAudioSource = audioSources[1];

        PlaySound("Level1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(String key)
    {
        foreach(SoundInfo s in sounds)
        {
            if (s.key.Equals(key))
            {
                if (s.isMusic)
                {
                    musicAudioSource.clip = s.value;
                    musicAudioSource.Play();
                }
                else
                {
                    soundAudioSource.clip = s.value;
                    soundAudioSource.Play();
                }
                break;
            }
        }
    }
}
