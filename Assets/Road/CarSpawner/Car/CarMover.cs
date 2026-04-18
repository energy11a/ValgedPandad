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

    [Header("Random logic")]
    public PersistentRandomSpace randomLogic;

    void Awake()
    {
        startScale = transform.localScale;
    }

    public void Init(Vector3 spawnPos, float angle, float scaleAngle)
    {
        // reset random state
        if (randomLogic != null)
            randomLogic.ResetChance();

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

        if (vp.x < -0.2f || vp.x > 1.2f || vp.y < -0.2f || vp.y > 1.2f)
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.IncrementScore(0.1f);
            }
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.Incrementcrashes();
                GameController.Instance.SetScore(0);
            }
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}