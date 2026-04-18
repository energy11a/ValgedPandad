using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PersistentRandomSpace : MonoBehaviour
{
    [Header("Visual")]
    public GameObject lights;

    [Range(0f, 1f)]
    public float chance = 0.5f;

    private bool? result = null;

    private bool hasHonkTarget = false;
    private float lightTimer = 0f;
    private float lightDuration = 0.5f;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AudioManager.instance.PlayRandom();
        }
        
        if (hasHonkTarget)
            
        {
            if (result == null)
            {
                result = Random.value < chance;
            }
            if (result == true)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    GameController.Instance.IncrementScore();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                //välguta tulesid
                //sisse
                if (lights != null)
                {
                    AudioManager.instance.Play("Signaal");
                    lights.SetActive(true);
                }
                    

                lightTimer = lightDuration;
            }
            

        }
        if (lightTimer > 0f)
        {
            lightTimer -= Time.deltaTime;

            if (lightTimer <= 0f)
            {
                if (lights != null)
                    lights.SetActive(false);
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