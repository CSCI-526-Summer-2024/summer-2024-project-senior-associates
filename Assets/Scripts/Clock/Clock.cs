using UnityEngine;
using TMPro;

public struct TimeSuffix
{
    public enum Value { AM, PM };
    public Value value;
    public override readonly string ToString()
    {
        return value == Value.AM ? "AM" : "PM";
    }
    public void Toggle()
    {
        if (value == Value.AM)
        {
            value = Value.PM;
        }
        else
        {
            value = Value.AM;
        }
    }
}

public class Clock : MonoBehaviour
{
    private const int StartHour = 9;
    private const int EndHour = 17;
    private const int TotalMinutes = (EndHour - StartHour) * 60;
    private const int TotalRealTimeInSeconds = 120;
    private const int MinuteIncrement = 15;
    private const float IncrementCountdownStart = TotalRealTimeInSeconds / (TotalMinutes / MinuteIncrement);
    private TMP_Text clock;
    private int hour = 9;
    private int minute = 0;
    private TimeSuffix suffix = new() { value = TimeSuffix.Value.AM };
    private float incrementCountdown = 0f;

    void Start()
    {
        clock = GetComponent<TMP_Text>();
        incrementCountdown = IncrementCountdownStart;
        UpdateText();
    }

    void FixedUpdate()
    {
        incrementCountdown -= Time.fixedDeltaTime;
        if (incrementCountdown <= 0f)
        {
            incrementCountdown = IncrementCountdownStart;
            IncrementTime();
        }
    }

    void IncrementTime()
    {
        minute += MinuteIncrement;
        if (minute >= 60)
        {
            minute -= 60;
            hour++;
            if (hour == 12)
            {
                suffix.Toggle();
            }
            else if (hour > 12)
            {
                hour -= 12;
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        var str = $"{hour}:";
        if (minute < 10)
        {
            str += "0";
        }
        str += $"{minute} {suffix}";
        clock.text = str;
    }
}
