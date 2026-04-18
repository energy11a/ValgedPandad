using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] RawImage targetImage;
    [SerializeField] Texture[] frames;
    [SerializeField] float frameRate = 12f;

    int currentFrame;
    float timer;

    void Update()
    {
        if (frames == null || frames.Length == 0 || targetImage == null)
            return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % frames.Length;
            targetImage.texture = frames[currentFrame];
        }
    }

    public void SetFrames(Texture[] newFrames)
    {
        frames = newFrames;
        currentFrame = 0;
        timer = 0f;
        if (targetImage != null && frames.Length > 0)
            targetImage.texture = frames[0];
    }

    public void SetFrameRate(float newFrameRate)
    {
        frameRate = newFrameRate;
    }
}
