using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
   [SerializeField] AudioMixer mixer;

    [Header("Panels")]
    [Tooltip("Kõik menüü paneelid (nt Main, LevelSelect, Settings jne)")]
    [SerializeField] GameObject[] panels;

    private const string LastPanelKey = "LastActivePanel";

    private void Start()
    {
        int savedIndex = PlayerPrefs.GetInt(LastPanelKey, 0);
        PlayerPrefs.DeleteKey(LastPanelKey);
        PlayerPrefs.Save();
        ShowPanel(savedIndex);
    }

    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
                panels[i].SetActive(i == index);
        }
    }

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
        SaveActivePanel();
        SceneManager.LoadScene(sceneName);
    }

    private void SaveActivePanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null && panels[i].activeSelf)
            {
                PlayerPrefs.SetInt(LastPanelKey, i);
                PlayerPrefs.Save();
                return;
            }
        }
    }
}
