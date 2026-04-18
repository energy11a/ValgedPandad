using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float moveAngle;

    private Vector3 moveDir;

    [Header("Scaling")]
    public float angleDegrees = 45f;
    private float scaleFactor;

    private float startY;
    private Vector3 startScale;

    void Awake()
    {
        // KORRA prefabi eluea jooksul
        startScale = transform.localScale;
    }

    // kutsu seda spawnerist
    public void Init(Vector3 spawnPos, float angle, float scaleAngle)
    {
        transform.position = spawnPos;

        moveAngle = angle;
        angleDegrees = scaleAngle;

        float rad = moveAngle * Mathf.Deg2Rad;
        moveDir = new Vector3(Mathf.Sin(rad), -Mathf.Cos(rad), 0f).normalized;

        startY = spawnPos.y;
        

        scaleFactor = Mathf.Sin(angleDegrees * Mathf.Deg2Rad);
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        float deltaY = startY - transform.position.y;

        float scaleDelta = deltaY * scaleFactor;

        transform.localScale = startScale + Vector3.one * scaleDelta;
        CheckOutOfScreen();
    }
    void CheckOutOfScreen()
    {
        if (Camera.main == null) return;

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);

        bool outOfBounds =
            vp.x < -0.2f || vp.x > 1.2f ||
            vp.y < -0.2f || vp.y > 1.2f;

        if (outOfBounds)
        {
            gameObject.SetActive(false); // object pool friendly
        }
    }
}