using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [Header("Views")]
    [Tooltip("Otse sinu poole sõitev vaade")]
    [SerializeField] GameObject frontView;
    [Tooltip("1 raja kaugusel olev külgvaade")]
    [SerializeField] GameObject sideViewNear;
    [Tooltip("2 raja kaugusel olev külgvaade")]
    [SerializeField] GameObject sideViewFar;
    [Tooltip("Välja signaalitud auto külgvaade")]
    [SerializeField] GameObject sideViewOut;
    public bool isOut = false;

    [Header("Kangekaelne (ei sõida teelt maha)")]
    public bool stubborn = false;

    [HideInInspector] public int laneIndex = 1;
    [HideInInspector] public float moveSpeed = 5f;
    [HideInInspector] public SimpleSpawner spawner;

    [Header("Scale Effect (Fake 3D)")]
    public float maxScale = 2.0f;

    [Header("Scale easing")]
    [Tooltip("How much of the journey (0-0.5) is used to scale up from 0")]
    public float scaleInPortion = 0.3f;
    [Tooltip("How much of the journey (0-0.5) is used to scale down to 0 at the end")]
    public float scaleOutPortion = 0.2f;

    [Header("Speed")]
    [Tooltip("Kiirusekordaja autodele, mis sõidavad kõrvalrajal (mitte sinu rajal)")]
    public float sideSpeedMultiplier = 2.5f;

    public float sideExitSpeed = 8f;
    float destroyTimer = 0f;
    float progress = 0f;
    int exitDirection = -1; // 1 = right, -1 = left
    bool hasPassedPlayer = false;
    public bool HasPassedPlayer => hasPassedPlayer;
    public float Progress => progress;

    void UpdateView()
    {
        int laneDist = Mathf.Abs(laneIndex - LaneController.CurrentLane);
        SetAllViewsInactive();

        if (laneDist == 0)
        {
            if (frontView != null) frontView.SetActive(true);
        }
        else if (laneDist == 1)
        {
            if (sideViewNear != null) sideViewNear.SetActive(true);
            MirrorView(sideViewNear, laneIndex < LaneController.CurrentLane ? 1 : -1);
        }
        else
        {
            if (sideViewFar != null) sideViewFar.SetActive(true);
            MirrorView(sideViewFar, laneIndex < LaneController.CurrentLane ? 1 : -1);
        }
    }

    void SetAllViewsInactive()
    {
        if (frontView != null) frontView.SetActive(false);
        if (sideViewNear != null) sideViewNear.SetActive(false);
        if (sideViewFar != null) sideViewFar.SetActive(false);
        if (sideViewOut != null) sideViewOut.SetActive(false);
    }

    void MirrorView(GameObject view, int direction)
    {
        if (view == null) return;
        Vector3 s = view.transform.localScale;
        s.x = Mathf.Abs(s.x) * direction;
        view.transform.localScale = s;
    }

    void Start()
    {
        transform.localScale = Vector3.zero;
        exitDirection = Random.value < 0.5f ? -1 : 1;
    }

    void Update()
    {
        if (stubborn) isOut = false;

        if (isOut)
        {
            SetAllViewsInactive();
            if (sideViewOut != null) sideViewOut.SetActive(true);

            // Mirror the side view on X axis based on exit direction
            Vector3 sideScale = sideViewOut != null ? sideViewOut.transform.localScale : Vector3.one;
            sideScale.x = Mathf.Abs(sideScale.x) * exitDirection;
            if (sideViewOut != null) sideViewOut.transform.localScale = sideScale;

            transform.Translate(Vector3.right * exitDirection * sideExitSpeed * Time.deltaTime, Space.World);

            destroyTimer += Time.deltaTime;
            if (destroyTimer > 3f)
                Destroy(gameObject);
        }
        else
        {
            if (spawner == null) { Destroy(gameObject); return; }

            // Get path from the CURRENT view for this car's fixed lane
            int currentView = LaneController.CurrentLane;
            spawner.GetLanePath(currentView, laneIndex, out Vector3 startPos, out Vector3 endPos);

            float totalDist = Vector3.Distance(startPos, endPos);
            if (totalDist < 0.001f) { Destroy(gameObject); return; }

            float speed = moveSpeed;
            if (laneIndex != LaneController.CurrentLane)
                speed *= sideSpeedMultiplier;

            progress += (speed / totalDist) * Time.deltaTime;
            progress = Mathf.Clamp01(progress);

            transform.position = Vector3.Lerp(startPos, endPos, progress);

            // Scale: 0 -> max (ease out) during scaleInPortion, hold, then max -> 0 during scaleOutPortion
            float scale;
            if (progress < scaleInPortion)
            {
                float t = progress / scaleInPortion;
                t = 1f - (1f - t) * (1f - t); // ease out (fast start, slow end)
                scale = Mathf.Lerp(0f, maxScale, t);
            }
            else if (progress > 1f - scaleOutPortion)
            {
                float t = (progress - (1f - scaleOutPortion)) / scaleOutPortion;
                t = t * t; // ease in (slow start, fast end)
                scale = Mathf.Lerp(maxScale, 0f, t);
            }
            else
            {
                scale = maxScale;
            }
            transform.localScale = new Vector3(scale, scale, 1f);

            UpdateView();

            // Auto on juba möödunud mängijast kui ta hakkab väiksemaks minema
            if (!hasPassedPlayer && progress > 1f - scaleOutPortion)
            {
                hasPassedPlayer = true;
                if (laneIndex == LaneController.CurrentLane)
                {
                    if (GameController.Instance != null)
                        GameController.Instance.Incrementcrashes();
                }
            }

            if (progress >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
