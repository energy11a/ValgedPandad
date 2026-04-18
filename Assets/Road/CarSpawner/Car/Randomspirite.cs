using UnityEngine;

public class RandomSpriteOnEnable : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] sprites;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (sr == null || sprites == null || sprites.Length == 0)
            return;

        int index = Random.Range(0, sprites.Length);
        sr.sprite = sprites[index];
    }
}