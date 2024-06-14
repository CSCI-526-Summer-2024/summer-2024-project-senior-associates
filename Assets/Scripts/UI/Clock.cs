using UnityEngine;
using TMPro;

public struct ClockTime
{
    public enum Suffix { AM, PM };
    public Suffix suffix;
    public int realHour;
    public int displayHour;
    public int minute;

    public ClockTime(int hour)
    {
        realHour = hour;
        displayHour = hour;
        minute = 0;
        suffix = Suffix.AM;
    }

    public override readonly string ToString()
    {
        var str = $"{displayHour}:";
        if (minute < 10)
        {
            str += "0";
        }
        str += $"{minute} {(suffix == Suffix.AM ? "AM" : "PM")}";
        return str;
    }

    public void AddMinutes(int numMinutes)
    {
        minute += numMinutes;
        if (minute >= 60)
        {
            minute -= 60;
            realHour++;
            displayHour++;
            if (displayHour == 12)
            {
                suffix = suffix == Suffix.AM ? Suffix.PM : Suffix.AM;
            }
            else if (displayHour > 12)
            {
                displayHour -= 12;
            }
        }
    }
}

public class Clock : MonoBehaviour
{
    public UIManager uiManager;
    public float TotalRealTimeInSeconds;
    private const int StartHour = 9;
    private const int EndHour = 17;
    private const int TotalMinutes = (EndHour - StartHour) * 60;
    private const int MinuteIncrement = 15;
    private float IncrementCountdownStart;
    private TMP_Text clockText;
    private ClockTime clockTime = new(StartHour);
    private float incrementCountdown = 0f;

    void Start()
    {
        clockText = GetComponent<TMP_Text>();
        clockText.text = $"{clockTime}";
        IncrementCountdownStart = TotalRealTimeInSeconds / (TotalMinutes / MinuteIncrement);
        incrementCountdown = IncrementCountdownStart;
    }

    void Update()
    {
        incrementCountdown -= Time.deltaTime;
        if (incrementCountdown <= 0f)
        {
            incrementCountdown = IncrementCountdownStart;
            clockTime.AddMinutes(MinuteIncrement);
            clockText.text = $"{clockTime}";

            if (clockTime.realHour == EndHour)
            {
                uiManager.EndLevel();
            }
        }
    }
}
