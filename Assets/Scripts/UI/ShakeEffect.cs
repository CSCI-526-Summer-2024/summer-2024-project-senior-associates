using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    private readonly float ShakeMagnitude = 0.08f;
    private Vector3 originalPosition;
    private float shakeInterval;
    private float shakeDurationCountdown;
    private float shakeIntervalCountdown;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDurationCountdown > 0f)
        {
            shakeDurationCountdown -= Time.deltaTime;
            if (shakeIntervalCountdown > 0f)
            {
                shakeIntervalCountdown -= Time.deltaTime;
                if (shakeIntervalCountdown <= 0f)
                {
                    float x = Random.Range(-1f, 1f) * ShakeMagnitude;
                    float y = Random.Range(-1f, 1f) * ShakeMagnitude;
                    transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
                    shakeIntervalCountdown = shakeInterval;
                }
            }

            if (shakeDurationCountdown <= 0f)
            {
                transform.localPosition = originalPosition;
            }
        }
    }

    public void Trigger(float duration, float interval)
    {
        shakeDurationCountdown = duration;
        shakeInterval = interval;
        shakeIntervalCountdown = 0f;
    }
}
