using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    [Header("Prefabs to spawn")]
    public List<GameObject> prefabs;

    [Header("Spawn points")]
    public List<Transform> spawnPoints;

    [Header("Pool settings")]
    public int initialPoolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Loome algse pooli
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObject();
        }
    }

    GameObject CreateNewObject()
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    GameObject GetPooledObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Kui kõik kasutuses → loo uus
        return CreateNewObject();
    }

    public void Spawn()
    {
        if (spawnPoints.Count == 0 || prefabs.Count == 0) return;

        float[] laneAngles = { -20f, 0f, 20f }; // vasak, kesk, parem

        GameObject obj = GetPooledObject();

        int lane = Random.Range(0, laneAngles.Length);

        var car = obj.GetComponent<CarMovement>();
        car.moveAngle = laneAngles[lane];
        car.Init(
    spawnPoints[lane].position,
    laneAngles[lane],
    20f
);

        obj.transform.position = spawnPoints[lane].position;

        obj.SetActive(true);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var point in spawnPoints)
        {
            if (point != null)
            {
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}