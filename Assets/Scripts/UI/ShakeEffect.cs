using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    private readonly float ShakeDuration = 0.5f;
    private readonly float ShakeMagnitude = 0.08f;
    private readonly float ShakeInterval = 0.02f;
    private Vector3 originalPosition;
    private float shakeTimeRemaining;
    private float shakeEffectCooldown;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0f)
        {
            shakeTimeRemaining -= Time.deltaTime;
            if (shakeEffectCooldown > 0f)
            {
                shakeEffectCooldown -= Time.deltaTime;
            }
            else
            {
                float x = Random.Range(-1f, 1f) * ShakeMagnitude;
                float y = Random.Range(-1f, 1f) * ShakeMagnitude;
                transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
                shakeEffectCooldown = ShakeInterval;
            }

            if (shakeTimeRemaining <= 0f)
            {
                transform.localPosition = originalPosition;
            }
        }
    }

    public void TriggerShake()
    {
        shakeTimeRemaining = ShakeDuration;
        shakeEffectCooldown = 0f;
    }
}
