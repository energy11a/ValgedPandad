using UnityEngine;
using UnityEngine.Audio;

public class Mainmenu : MonoBehaviour
{
   [SerializeField] AudioMixer mixer;

    public void ChangeVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }

    private void Update()
    {
        AudioManager.instance.PlayRandomSounds(new string[] { "Flute", "Start_Mahedam", "Start_Pulse" });
    }
    
}
