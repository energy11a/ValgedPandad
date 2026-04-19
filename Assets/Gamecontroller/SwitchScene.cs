using System;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] int sceneIndex = 0;
    
    public void SwitchToScene()
    {
        try
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load scene with index " + sceneIndex + ": " + ex.Message);
        }
    }
}
