using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLaneController : MonoBehaviour
{
    [Header("Lane X positions")]
    public List<float> laneX;

    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Rotation")]
    public float baseTiltX = -50f;     // alla vaatamine
    public float maxTiltZ = 10f;      // kallutus vasak/parem

    private int currentLane = 1;

    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
            Keyboard.current.aKey.wasPressedThisFrame)
        {
            currentLane = Mathf.Max(0, currentLane - 1);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame)
        {
            currentLane = Mathf.Min(laneX.Count - 1, currentLane + 1);
        }

        float targetX = laneX[currentLane];

        // POSITION
        Vector3 targetPos = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // --- ROTATION (head tilt effect) ---

        // kui palju oleme keskmest eemal (-1, 0, 1)
        float center = (laneX.Count - 1) / 2f;
        float offset = currentLane - center;

        float normalized = 0f;
        if (laneX.Count > 1)
            normalized = offset / center; // -1 .. 1

        float tiltZ = -normalized * maxTiltZ;

        Quaternion targetRot = Quaternion.Euler(
            baseTiltX,
            0f,
            tiltZ
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            10f * Time.deltaTime
        );
    }
}