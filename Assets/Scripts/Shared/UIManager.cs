using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gainedScoreAboveManagerPrefab;
    public int currentScore = 0;
    public Canvas canvas;

    public void AddScore(int score, Vector3 position)
    {
        currentScore += score;
        GameObject text = Instantiate(gainedScoreAboveManagerPrefab, position, Quaternion.identity);
        text.transform.SetParent(canvas.transform);
        text.GetComponent<GainedScoreText>().SetText(GetGainedScoreMessage(score));
    }

    private string GetGainedScoreMessage(int score)
    {
        return $"+{score} KPI!";
    }

}