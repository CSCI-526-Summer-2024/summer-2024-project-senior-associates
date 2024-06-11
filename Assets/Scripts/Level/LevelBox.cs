using TMPro;
using UnityEngine;

public class LevelBox : MonoBehaviour
{
    public TMP_Text levelNumText;
    public TMP_Text bestScoreText;
    public Canvas canvas;
    public ClickableRect clickableRect;
    private readonly float CanvasYOffsetWithoutBestScore = 0f;
    private readonly float CanvasYOffsetWithBestScore = 0.18f;

    public void SetLevelNumAndBestScore(int levelNum, BestKpi bestScore)
    {
        clickableRect.levelNum = levelNum;
        levelNumText.text = $"{levelNum}";
        if (!bestScore.present)
        {
            bestScoreText.text = "";
            canvas.transform.localPosition = Util.ChangeY(canvas.transform.localPosition, CanvasYOffsetWithoutBestScore);
        }
        else
        {
            bestScoreText.text = $"{bestScore}";
            canvas.transform.localPosition = Util.ChangeY(canvas.transform.localPosition, CanvasYOffsetWithBestScore);
        }
    }

}