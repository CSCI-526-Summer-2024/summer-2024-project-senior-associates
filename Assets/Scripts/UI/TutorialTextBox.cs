using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTextBox : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public TMP_Text mainText;
    public CircularProgress circularProgress;
    public PlayerControl playerControl;
    private readonly Vector3 MainTextOffsetWithoutSkip = new(0f, 0f, 0f);
    private readonly Vector3 MainTextOffsetWithSkip = new(0f, 0.0705f, 0f);

    public void SetContents(string text, bool showPressToSkip = false)
    {
        mainText.transform.localPosition = showPressToSkip ? MainTextOffsetWithSkip : MainTextOffsetWithoutSkip;
        mainText.text = text;

        circularProgress.gameObject.SetActive(showPressToSkip);
        playerControl.disableJump = showPressToSkip;

        mainText.GetComponent<FlickerEffect>().TriggerFlicker();
    }

    public void Hide()
    {
        playerControl.disableJump = false;
        gameObject.SetActive(false);
    }
}
