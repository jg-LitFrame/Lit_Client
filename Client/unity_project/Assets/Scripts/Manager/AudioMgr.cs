using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lit.Unity;

[RequireComponent(typeof(AudioSource))]
public class AudioMgr : SingletonBehaviour<AudioMgr>
{
    private string audioRootPath = "Sounds/";
    private Dictionary<string, AudioClip> audiosCache = new Dictionary<string, AudioClip>();
    private AudioSource audioPlayer;

    protected override void Init()
    {
        if (audioPlayer == null)
        {
            audioPlayer = GetOrAddComponent<AudioSource>();
            if (audioPlayer == null)
                LitLogger.Error("Attach AudioSource Faild !!");
        }
    }

    public void PlayAudio(string audio_name, bool isLoop = false)
    {
        AudioClip clip = GetClip(audio_name);
        if (clip == null)
        {
            LitLogger.ErrorFormat("Can'not Found {0} audio", audio_name);
            return;
        }
        audioPlayer.clip = clip;
        audioPlayer.Play();
    }
    private AudioClip GetClip(string audio_name)
    {
        AudioClip clip = GetClipInCache(audio_name);
        if (clip == null)
            clip = LoadAndCacheAudioClip(audio_name);
        return clip;
    }

    private AudioClip GetClipInCache(string audio_name)
    {
        AudioClip clip = null;
        audiosCache.TryGetValue(audio_name, out clip);
        return clip;
    }

    private AudioClip LoadAndCacheAudioClip(string audio_name)
    {
        string audio_path = audioRootPath + audio_name;
        AudioClip clip = Resources.Load<AudioClip>(audio_path);
        if (clip != null)
            audiosCache.Add(audio_name, clip);
        return clip;
    }
}


