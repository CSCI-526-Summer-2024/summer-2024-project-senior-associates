using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMood : MonoBehaviour
{
    public GameObject moodBar;
    private readonly float MoodMax = 1f;
    private readonly float PenaltyMultiplier = -0.5f;  // penalty = request.reward * PenaltyMultiplier
    private float mood = 1f; public float Mood => mood;
    private float moodDropSpeed = 0f;
    private Manager manager;

    void Awake()
    {
        manager = GetComponent<Manager>();
    }

    void Update()
    {
        if (manager.Request == null)
        {
            moodBar.SetActive(false);
        }
        else
        {
            moodBar.SetActive(true);
            moodBar.transform.localScale = Util.ChangeX(moodBar.transform.localScale, mood / MoodMax);
            moodBar.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, mood);
            mood -= moodDropSpeed * Time.deltaTime;
            if (mood < 0f)
            {
                manager.uiManager.UpdateScore((int)Math.Round(PenaltyMultiplier * manager.Request.reward), transform.position);
                manager.FinishRequest();
            }
        }
    }

    public void Reset()
    {
        mood = 1f;
    }

    public void SetRequestMaxTime(float requestMaxTime)
    {
        moodDropSpeed = 1 / requestMaxTime;
    }
}
