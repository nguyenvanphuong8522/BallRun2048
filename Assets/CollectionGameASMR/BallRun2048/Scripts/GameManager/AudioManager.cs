using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource[] soundSources;
    private Queue<AudioSource> _queueSources;

    public List<Sound> sfxs;
    public List<Sound> musics;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            _queueSources = new Queue<AudioSource>(soundSources);
        }
        DontDestroyOnLoad(this);
    }

    public void ChangeSoundVolume()
    {
        foreach (var sound in soundSources)
        {
            if (sound.volume > 0)
                sound.volume = 0;
            else
                sound.volume = 1;
        }
    }

    public void PlayShot(string name)
    {
        Sound sound = sfxs.Find(x => x.name == name);
        if (sound == null)
        {
            return;
        }
        var source = _queueSources.Dequeue();
        if (!source)
        {
            return;
        }
        source.PlayOneShot(sound.clip);
        _queueSources.Enqueue(source);
    }
}