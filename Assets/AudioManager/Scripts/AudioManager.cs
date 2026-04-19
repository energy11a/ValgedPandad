using UnityEngine.Audio;
using System;
using UnityEngine;
using JetBrains.Annotations;


public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    [SerializeField] AudioMixerGroup mixer, sfx, music;
    public static AudioManager instance;

    void Awake() 
    {
        CreateInstance();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = 0f;
            s.source.loop = s.loop;
            if(s.isSFX)
            {
                s.source.outputAudioMixerGroup = sfx;
            }
            else
            {
                s.source.outputAudioMixerGroup = music;
            }
        }
    }
    void CreateInstance()
    {
        if (instance == null)
        instance = this;
    }
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        // kui juba mängib ära tee midagi
        if (s.source.isPlaying)
            return;
        s.source.Play();
    }
    public void PlayRandomSounds(string[] names)
    {
        if (names == null || names.Length == 0)
            return;

        if (IsAnyPlaying())
            return;

        string randomName = names[UnityEngine.Random.Range(0, names.Length)];
        Sound s = Array.Find(sounds, sound => sound.name == randomName);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + randomName);
            return;
        }

        s.source.Play();
    }
    public void StopAndPlayRandom(string[] names)
    {
        if (names == null || names.Length == 0)
            return;

        // Stop only currently playing SFX sounds
        foreach (Sound s in sounds)
        {
            if (s.isSFX && s.source != null && s.source.isPlaying)
                s.source.Stop();
        }

        string randomName = names[UnityEngine.Random.Range(0, names.Length)];
        Sound s2 = Array.Find(sounds, sound => sound.name == randomName);
        if (s2 == null)
        {
            Debug.LogWarning("Sound not found: " + randomName);
            return;
        }

        s2.source.Play();
    }

    public bool IsAnyPlaying()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null && s.source.isPlaying)
                return true;
        }
        return false;
    }

}