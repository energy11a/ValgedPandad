using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public SimpleSpawner spawner;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float startDelay = 1f;

    [Header("Score")]
    public float score = 0f;
    public float crashes = 0f;

    public static GameController Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), startDelay, spawnInterval);
    }

    void SpawnObject()
    {
        if (spawner != null)
        {
            spawner.Spawn();
        }
    }

    // ---------------- SCORE API ----------------

    public void IncrementScore(float amount = 1)
    {
        score += amount;
        LogScore("INCREASE");
    }

    public void Incrementcrashes(float amount = 1)
    {
        crashes += amount;
    }

    public void DecrementScore(float amount = 1)
    {
        score -= amount;
        LogScore("DECREASE");
    }

    public void SetScore(float value)
    {
        score = value;
        LogScore("SET");
    }
    // ---------------- DEV LOG ----------------

    void LogScore(string action)
    {
        Debug.Log($"[SCORE {action}] Current score: {score}");
    }
}