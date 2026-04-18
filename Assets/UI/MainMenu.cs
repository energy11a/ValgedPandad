using UnityEngine;
using UnityEngine.Audio;

public class Mainmenu : MonoBehaviour
{
   [SerializeField] AudioMixer mixer;

    public void ChangeVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }

    private void Awake()
    {
        AudioManager.instance.Play("Tetris");
    }

}
