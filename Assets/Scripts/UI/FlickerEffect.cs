using UnityEngine;
using TMPro;

public class FlickerEffect : MonoBehaviour
{
    private const float FlickerDuration = 0.6f;
    private const float FlickerInterval = 0.15f;
    private const float UpdateOpacitySpeed = 1f / FlickerInterval;
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
                    flickerIntervalCountdown = FlickerInterval;
                    isGettingBrighter = !isGettingBrighter;
                }
            }
            UpdateTextOpacity(UpdateOpacitySpeed * Time.deltaTime * (isGettingBrighter ? 1 : -1));
            
            if (flickerDurationCountdown <= 0f)
            {
                UpdateTextOpacity(1f);
            }
        }
    }

    public void TriggerFlicker()
    {
        flickerDurationCountdown = FlickerDuration;
        flickerIntervalCountdown = FlickerInterval;
        isGettingBrighter = false;
    }

    private void UpdateTextOpacity(float deltaOpacity)
    {
        var currentColor = tmp.color;
        currentColor.a = Mathf.Clamp01(currentColor.a + deltaOpacity);
        tmp.color = currentColor;
    }
}
