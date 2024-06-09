using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gainedScoreAboveManagerPrefab;
    public CurrentScoreText currentScoreText;
    public Canvas canvas;

    public void AddScore(int score, Vector3 position)
    {
        currentScoreText.AddScore(score);
        CreateGainedScoreText(score, position + new Vector3(0, 0.5f, 0f));
        CreateGainedScoreText(score, currentScoreText.GetBottomRightPoint() + new Vector3(1f, -0.2f, 0f));
    }

    private string GetGainedScoreMessage(int score)
    {
        return $"+{score} KPI!";
    }

    private void CreateGainedScoreText(int score, Vector3 position)
    {
        GameObject text = Instantiate(gainedScoreAboveManagerPrefab, position, Quaternion.identity);
        text.transform.SetParent(canvas.transform);
        text.GetComponent<GainedScoreText>().SetText(GetGainedScoreMessage(score));
    }

}