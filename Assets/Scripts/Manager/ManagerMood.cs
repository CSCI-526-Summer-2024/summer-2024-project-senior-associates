using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMood : MonoBehaviour
{
    public GameObject moodBar;
    private float productCountdown = 1f;
    public float Value => productCountdown;

    private float productCountdownMax = 1f;
    private float moodDrop;

    private Gradient moodGradient;

    private GradientColorKey[] moodColors;
    private GradientAlphaKey[] moodAlpha;
    // Start is called before the first frame update
    void Start()
    {
        //setting up gradient
        moodDrop = 0.01f * Time.deltaTime;
        moodGradient = new Gradient();

        moodColors = new GradientColorKey[2];
        moodColors[0] = new GradientColorKey(Color.red, 0.0f);
        moodColors[1] = new GradientColorKey(Color.green, 1.0f);

        moodAlpha = new GradientAlphaKey[2];
        moodAlpha[0] = new GradientAlphaKey(1.0f, 0.0f);
        moodAlpha[1] = new GradientAlphaKey(1.0f, 0.0f);
        moodGradient.SetKeys(moodColors, moodAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (productCountdown > 0f)
        {
            Vector3 scale = moodBar.transform.localScale;
            scale.x = productCountdown / productCountdownMax;
            moodBar.transform.localScale = scale;

            productCountdown -= moodDrop;

            if (productCountdown > 0f) //&& productCountdown < productCountdownMax* 0.5)
            {
                //moodBar.GetComponent<Image>().color=Color.yellow;
                moodBar.GetComponent<Image>().color = moodGradient.Evaluate(productCountdown);
            }
        }
        else
        {
            productCountdown = 0f;
            //moodBar.GetComponent<Image>().color=Color.red;
        }
    }

    public void updateMood()
    {
        productCountdown = 1f;
    }
}
