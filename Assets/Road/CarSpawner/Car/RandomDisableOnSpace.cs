using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PersistentRandomSpace : MonoBehaviour
{
    [Range(0f, 1f)]
    public float chance = 0.5f;

    private bool? result = null;

    private bool hasHonkTarget = false;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (result == null)
            {
                result = Random.value < chance;
            }

            if (result == true)
            {
                if (hasHonkTarget)
                {
                    GameController.Instance.IncrementScore();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HonkAOE"))
        {
            hasHonkTarget = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HonkAOE"))
        {
            hasHonkTarget = false;
        }
    }

    public void ResetChance()
    {
        result = null;
        hasHonkTarget = false;
    }
}