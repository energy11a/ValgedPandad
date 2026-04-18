using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaneController : MonoBehaviour
{
    [Header("Lane X offsets (applied to road container)")]
    public List<float> laneX;

    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Road container (parent of spawn points & cars)")]
    public Transform roadContainer;

    [Header("Background GameObjects (one per lane: left, center, right)")]
    public List<GameObject> laneBackgrounds;

    public static int CurrentLane { get; private set; } = 1;

    void Start()
    {
        SetActiveBackground(CurrentLane);
    }

    void Update()
    {
        int previousLane = CurrentLane;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
            Keyboard.current.aKey.wasPressedThisFrame)
        {
            CurrentLane = Mathf.Max(0, CurrentLane - 1);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame)
        {
            CurrentLane = Mathf.Min(laneX.Count - 1, CurrentLane + 1);
        }

        if (CurrentLane != previousLane)
        {
            SetActiveBackground(CurrentLane);
        }

        // Shift road container opposite to player lane so cars stay in correct lanes
        if (roadContainer != null)
        {
            float targetX = -laneX[CurrentLane];

            Vector3 targetPos = new Vector3(
                targetX,
                roadContainer.position.y,
                roadContainer.position.z
            );

            roadContainer.position = Vector3.Lerp(
                roadContainer.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void SetActiveBackground(int lane)
    {
        for (int i = 0; i < laneBackgrounds.Count; i++)
        {
            if (laneBackgrounds[i] != null)
                laneBackgrounds[i].SetActive(i == lane);
        }
    }
}