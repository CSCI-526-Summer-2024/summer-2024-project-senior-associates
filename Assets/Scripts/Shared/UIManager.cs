using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gainedScoreAboveManagerPrefab;
    public CurrentScoreText currentScoreText;
    public Canvas canvas;

    public void UpdateScore(int score, Vector3 position)
    {
        currentScoreText.UpdateScore(score);
        CreateGainedScoreText(score, position + new Vector3(0, 0.5f, -1f));
        CreateGainedScoreText(score, currentScoreText.GetBottomRightPoint() + new Vector3(1f, -0.2f, -1f));
    }

    private string GetGainedScoreMessage(int score)
    {
        if (score >= 0)
        {
            return $"+{score} KPI!";
        }
        else
        {
            return $"{score} KPI";
        }
    }

    private void CreateGainedScoreText(int score, Vector3 position)
    {
        GameObject text = Instantiate(gainedScoreAboveManagerPrefab, position, Quaternion.identity);
        text.transform.SetParent(canvas.transform);
        text.GetComponent<GainedScoreText>().SetText(GetGainedScoreMessage(score));
    }

}