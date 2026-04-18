using UnityEngine.Audio;
using System;
using UnityEngine;
using JetBrains.Annotations;


public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    [SerializeField] AudioMixerGroup mixer;
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
            s.source.outputAudioMixerGroup = mixer;

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


}