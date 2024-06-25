using TMPro;
using UnityEngine;

public class TutorialTextBox : MonoBehaviour
{
    public TMP_Text mainText;
    public TMP_Text holdToSkipText;
    public CircularProgress circularProgress;
    public PlayerControl playerControl;
    private readonly Vector3 MainTextOffsetWithoutSkip = new(0f, -0.18f, 0f);
    private readonly Vector3 MainTextOffsetWithSkip = new(0f, -0.08f, 0f);

    public void SetContents(string text, bool showPressToSkip = false)
    {
        if (text != null)
        {
            gameObject.SetActive(true);
            mainText.transform.localPosition = showPressToSkip ? MainTextOffsetWithSkip : MainTextOffsetWithoutSkip;
            mainText.text = text;
            mainText.GetComponent<FlickerEffect>().Trigger(0.6f, 0.15f);

            circularProgress.gameObject.SetActive(showPressToSkip);
            playerControl.disableJump = showPressToSkip;
            if (showPressToSkip)
            {
                holdToSkipText.GetComponent<FlickerEffect>().Trigger(6f, 0.3f);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Hide()
    {
        playerControl.disableJump = false;
        gameObject.SetActive(false);
    }
}
