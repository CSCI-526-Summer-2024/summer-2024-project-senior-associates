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
    private SpriteRenderer circleSR;

    void Awake()
    {
        manager = GetComponent<Manager>();
        circleSR = manager.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (manager.InTutorial || manager.Request == null)
        {
            if (moodBar != null) moodBar.SetActive(false);
        }
        else
        {
            moodBar.SetActive(true);
            moodBar.transform.localScale = Util.ChangeX(moodBar.transform.localScale, mood / MoodMax);
            moodBar.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, mood);
            UpdateManagerColor();

            mood -= moodDropSpeed * Time.deltaTime;
            if (mood < 0f)
            {
                manager.uiManager.UpdateScore(false, (int)Math.Round(PenaltyMultiplier * manager.Request.reward), transform.position);
                manager.FinishRequest();
            }
        }
    }

    public void Reset()
    {
        mood = 1f;
        UpdateManagerColor();
    }

    public void SetRequestMaxTime(float requestMaxTime)
    {
        moodDropSpeed = 1 / requestMaxTime;
    }

    private void UpdateManagerColor()
    {
        circleSR.color = Color.Lerp(Color.red, Color.green, mood);
    }
}
