using UnityEngine;
using UnityEngine.InputSystem;

public class DisableOnSpace_NewInput : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            gameObject.SetActive(false);
        }
    }
}