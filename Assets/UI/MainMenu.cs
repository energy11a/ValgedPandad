using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
   [SerializeField] AudioMixer mixer;

    public void ChangeVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }
    public void ChangeVolumSFXe(float volume)
    {
        mixer.SetFloat("SFXVolume", volume);
    }
    public void ChangeVolumeMusic(float volume)
    {
        mixer.SetFloat("MusicVolume", volume);
    }
    private void Update()
    {
        AudioManager.instance.PlayRandomSounds(new string[] { "Flute", "Start_Mahedam", "Start_Pulse" });
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
