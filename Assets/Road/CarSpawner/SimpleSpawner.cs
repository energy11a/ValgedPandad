using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadView
{
    [Tooltip("Start points for each lane in this view")]
    public List<Transform> startPoints;
    [Tooltip("End points for each lane in this view")]
    public List<Transform> endPoints;
}

public class SimpleSpawner : MonoBehaviour
{
    [Header("Prefabs to spawn (picked randomly)")]
    public List<GameObject> carPrefabs;

    [Header("Road views (one per player lane: left, center, right)")]
    [Tooltip("Each view has start/end points for all 3 lanes from that perspective")]
    public List<RoadView> roadViews;

    [Header("Car speed towards the player")]
    public float carSpeed = 5f;

    public int LaneCount
    {
        get
        {
            if (roadViews == null || roadViews.Count == 0) return 3;
            return roadViews[0].startPoints.Count;
        }
    }

    // Keep old property for LaneController compatibility
    public List<Transform> spawnPoints
    {
        get
        {
            if (roadViews != null && roadViews.Count > 0)
                return roadViews[0].startPoints;
            return new List<Transform>();
        }
    }

    public void GetLanePath(int viewIndex, int laneIndex, out Vector3 start, out Vector3 end)
    {
        viewIndex = Mathf.Clamp(viewIndex, 0, roadViews.Count - 1);
        RoadView view = roadViews[viewIndex];
        laneIndex = Mathf.Clamp(laneIndex, 0, view.startPoints.Count - 1);
        start = view.startPoints[laneIndex].position;
        end = view.endPoints[laneIndex].position;
    }

    public void Spawn()
    {
        if (carPrefabs.Count == 0 || roadViews.Count == 0) return;

        int currentView = LaneController.CurrentLane;
        currentView = Mathf.Clamp(currentView, 0, roadViews.Count - 1);
        RoadView view = roadViews[currentView];

        if (view.startPoints.Count == 0 || view.endPoints.Count == 0) return;

        int lane = Random.Range(0, view.startPoints.Count);
        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];

        Vector3 spawnPos = view.startPoints[lane].position;
        GameObject car = Instantiate(prefab, spawnPos, Quaternion.identity, transform);

        CarBehaviour cb = car.GetComponent<CarBehaviour>();
        if (cb != null)
        {
            cb.laneIndex = lane;
            cb.moveSpeed = carSpeed;
            cb.spawner = this;
        }
    }
}
