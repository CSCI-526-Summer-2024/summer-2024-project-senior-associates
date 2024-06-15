using TMPro;
using UnityEngine;

public class LevelBox : MonoBehaviour
{
    public TMP_Text levelNumText;
    public TMP_Text bestKpiText;
    public Canvas canvas;
    public LevelBoxInnerRect clickableRect;
    private readonly float CanvasYOffsetWithoutBestScore = 0f;
    private readonly float CanvasYOffsetWithBestScore = 0.18f;

    public void Init(int levelNum, LevelInfo levelInfo)
    {
        clickableRect.levelNum = levelNum;
        if (!levelInfo.unlocked)
        {
            clickableRect.Disable();
        }
        levelNumText.text = $"{levelNum}";
        if (!levelInfo.played)
        {
            bestKpiText.text = "";
            canvas.transform.localPosition = Util.ChangeY(canvas.transform.localPosition, CanvasYOffsetWithoutBestScore);
        }
        else
        {
            bestKpiText.text = levelInfo.GetBestKpiString();
            canvas.transform.localPosition = Util.ChangeY(canvas.transform.localPosition, CanvasYOffsetWithBestScore);
        }
    }

}