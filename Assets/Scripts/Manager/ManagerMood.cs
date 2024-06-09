using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMood : MonoBehaviour
{
    public GameObject moodBar;
    private float mood = 1f;
    private float moodMax = 1f;
    public float Value => mood;
    private float moodDrop;

    // Start is called before the first frame update
    void Start()
    {
        moodDrop = 0.01f * Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (mood > 0f)
        {
            Vector3 scale = moodBar.transform.localScale;
            scale.x = mood / moodMax;
            moodBar.transform.localScale = scale;

            mood -= moodDrop;

            if (mood > 0f)
            {
                moodBar.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, mood);
            }
        }
        else
        {
            mood = 0f;
        }
    }

    public void updateMood()
    {
        mood = 1f;
    }
}
