using UnityEngine;
using UnityEngine.UI;

public delegate void OnCircularProgressDone();

public class CircularProgress : MonoBehaviour
{
    public OnCircularProgressDone onCircularProgressDone;
    public TutorialManager tutorialManager;
    public Image circularImage;
    private const KeyCode KeyToHold = KeyCode.Space;
    private const float FillTime = 1f;
    private const float FillSpeed = 1f / FillTime;
    private bool waitForKeyUp = false;

    void Start()
    {
        if (circularImage.type != Image.Type.Filled || circularImage.fillMethod != Image.FillMethod.Radial360)
        {
            Debug.LogWarning("Image type is not set to Filled with Radial 360. Adjusting settings.");
            circularImage.type = Image.Type.Filled;
            circularImage.fillMethod = Image.FillMethod.Radial360;
        }
        circularImage.fillAmount = 0f;
    }

    void Update()
    {
        if (waitForKeyUp && Input.GetKeyUp(KeyToHold))
        {
            waitForKeyUp = false;
        }
        else if (!waitForKeyUp && Input.GetKey(KeyToHold))
        {
            circularImage.fillAmount += FillSpeed * Time.deltaTime;
            if (circularImage.fillAmount >= 1f)
            {
                circularImage.fillAmount = 0f;
                onCircularProgressDone();
                waitForKeyUp = true;
            }
        }
        else
        {
            circularImage.fillAmount = 0f;
        }
    }
}
