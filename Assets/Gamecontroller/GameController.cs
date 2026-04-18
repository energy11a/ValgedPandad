using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public SimpleSpawner spawner;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float startDelay = 1f;

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
}