using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{

    public TMP_Text clock;

    private int hour;
    private int minute;

    private string ampm;

    private float clockInterval = 1f;
    private float time;

    public SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        hour = 9;
        minute = 0;
        ampm = " AM";
        clock.text = hour + ":0" + minute + ampm;
        time = 0f;
        sceneController = new SceneController();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time >= clockInterval)
        {
            updateTime();
            time = 0;
        }
    }

    void updateTime()
    {
        minute += 15;
        if (minute >= 60)
        {
            minute = 0;
            hour += 1;
        }
        if (hour > 11)
        {
            if (hour > 12)
            {
                hour = 1;
            }
            ampm = " PM";
        }
        if (minute < 10)
        {
            clock.text = hour + ":0" + minute + ampm;
        }
        else
        {
            clock.text = hour + ":" + minute + ampm;
        }

        // if (hour == 5)
        // {
        //     sceneController.SceneUpdate(1);
        //     //SceneManager.LoadScene("FinishScene",LoadSceneMode.Single);
        //     //SceneManager.SetActiveScene(SceneManager.GetSceneByName("FinishScene"));
        // }
    }
}
