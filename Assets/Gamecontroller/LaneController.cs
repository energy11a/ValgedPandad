using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaneController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Spawner (lane positions come from its roadViews)")]
    public SimpleSpawner spawner;

    [Header("Background GameObjects (one per lane: left, center, right)")]
    public List<GameObject> laneBackgrounds;

    [SerializeField] string[] honkNames;
    [SerializeField] GameObject hand;
    [SerializeField] float handShowDuration = 0.3f;

    public static int CurrentLane { get; private set; } = 1;

    int LaneCount => spawner != null ? spawner.LaneCount : 3;
    Coroutine handCoroutine;

    void Start()
    {
        if (hand != null) hand.SetActive(false);
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
            CurrentLane = Mathf.Min(LaneCount - 1, CurrentLane + 1);
        }

        if (CurrentLane != previousLane)
        {
            SetActiveBackground(CurrentLane);
        }

        // Space bar: push the nearest car in the current lane off the road
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AudioManager.instance.StopAndPlayRandom(honkNames);

            if (hand != null)
            {
                if (handCoroutine != null) StopCoroutine(handCoroutine);
                hand.SetActive(true);
                handCoroutine = StartCoroutine(HideHandAfterDelay());
            }

            CarBehaviour nearest = FindNearestCarInLane(CurrentLane);
            if (nearest != null && !nearest.isOut)
            {
                nearest.isOut = true;
                if (GameController.Instance != null)
                    GameController.Instance.IncrementScore();
            }
        }
    }

    IEnumerator HideHandAfterDelay()
    {
        yield return new WaitForSeconds(handShowDuration);
        if (hand != null) hand.SetActive(false);
    }

    CarBehaviour FindNearestCarInLane(int lane)
    {
        CarBehaviour[] cars = FindObjectsByType<CarBehaviour>(FindObjectsSortMode.None);
        CarBehaviour nearest = null;
        float highestProgress = -1f;

        foreach (CarBehaviour car in cars)
        {
            if (car.laneIndex == lane && !car.isOut && !car.HasPassedPlayer)
            {
                if (car.Progress > highestProgress)
                {
                    highestProgress = car.Progress;
                    nearest = car;
                }
            }
        }
        return nearest;
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