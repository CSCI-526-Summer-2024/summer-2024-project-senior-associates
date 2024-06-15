using UnityEngine;
using TMPro;

public class FlickerEffect : MonoBehaviour
{
    private float flickerInterval;
    private float flickerDurationCountdown = 0f;
    private float flickerIntervalCountdown = 0f;
    private bool isGettingBrighter = false;
    private TextMeshProUGUI tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (flickerDurationCountdown > 0f)
        {
            flickerDurationCountdown -= Time.deltaTime;
            if (flickerIntervalCountdown > 0f)
            {
                flickerIntervalCountdown -= Time.deltaTime;
                if (flickerIntervalCountdown <= 0f)
                {
                    flickerIntervalCountdown = flickerInterval;
                    isGettingBrighter = !isGettingBrighter;
                }
            }
            UpdateTextOpacity(1f / flickerInterval * Time.deltaTime * (isGettingBrighter ? 1 : -1));

            if (flickerDurationCountdown <= 0f)
            {
                UpdateTextOpacity(1f);
            }
        }
    }

    public void Trigger(float duration, float interval)
    {
        flickerDurationCountdown = duration;
        flickerInterval = interval;
        flickerIntervalCountdown = interval;
        isGettingBrighter = false;
    }

    private void UpdateTextOpacity(float deltaOpacity)
    {
        var currentColor = tmp.color;
        currentColor.a = Mathf.Clamp01(currentColor.a + deltaOpacity);
        tmp.color = currentColor;
    }
}
