using UnityEngine;
using TMPro;

public class FlickerEffect : MonoBehaviour
{
    private float interval;
    private float flickerDurationCountdown = 0f;
    private float flickerTimer = 0f;
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
            flickerTimer += Time.deltaTime;
            if (flickerTimer < interval)
            {
                UpdateTextOpacity(-1f / interval * Time.deltaTime);
            }
            else if (flickerTimer < interval * 2)
            {
                UpdateTextOpacity(1f / interval * Time.deltaTime);
            }
            else
            {
                SetTextOpacity(1f);
            }
            if (flickerTimer > interval * 4)
            {
                flickerTimer -= interval * 4;
            }
        }
        else
        {
            SetTextOpacity(1f);
        }
    }

    public void Trigger(float duration, float interval)
    {
        flickerDurationCountdown = duration;
        this.interval = interval;
        flickerTimer = 0f;
    }

    private void SetTextOpacity(float opacity)
    {
        var currentColor = tmp.color;
        currentColor.a = opacity;
        tmp.color = currentColor;
    }
    private void UpdateTextOpacity(float deltaOpacity)
    {
        SetTextOpacity(Mathf.Clamp01(tmp.color.a + deltaOpacity));
    }

}
