using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData
{
    public int deliveredNum = 0;
    public int deliveredKpi = 0;
    public int failedNum = 0;
    public int failedKpi = 0;
}

public class UIManager : MonoBehaviour
{
    public GameObject gainedScoreAboveManagerPrefab;
    public CurrentScoreText currentScoreText;
    public Canvas canvas;
    public TMP_Text minKpiText;
    private PlayerData playerData;
    private readonly LevelData levelData = new();
    private int levelNum;

    void Start()
    {
        levelNum = Util.GetCurrentLevelNum() - 1;
        playerData = PlayerData.LoadPlayerData();
        minKpiText.text = $"Goal: {playerData.levelInfos[levelNum].minKpi}";
    }

    public void UpdateScore(int score, Vector3 position)
    {
        if (score > 0)
        {
            levelData.deliveredNum++;
            levelData.deliveredKpi += score;
        }
        else
        {
            levelData.failedNum++;
            levelData.failedKpi += score;
        }

        currentScoreText.UpdateScore(score);
        CreateGainedScoreText(score, position + new Vector3(0, 0.5f, -1f));
        CreateGainedScoreText(score, currentScoreText.GetBottomRightPoint() + new Vector3(1f, -0.2f, -1f));
    }

    public void EndLevel()
    {
        var totalKpi = levelData.deliveredKpi + levelData.failedKpi;
        playerData.levelInfos[levelNum].UpdateBestKpi(totalKpi);
        if (levelNum + 1 < playerData.levelInfos.Count && totalKpi >= playerData.levelInfos[levelNum].minKpi)
        {
            playerData.levelInfos[levelNum + 1].unlocked = true;
        }
        playerData.SavePlayerData();

        PlayerPrefs.SetInt("MinKpi", playerData.levelInfos[levelNum].minKpi);
        PlayerPrefs.SetInt("DeliveredNum", levelData.deliveredNum);
        PlayerPrefs.SetInt("DeliveredKPI", levelData.deliveredKpi);
        PlayerPrefs.SetInt("FailedNum", levelData.failedNum);
        PlayerPrefs.SetInt("FailedKPI", levelData.failedKpi);
        Util.EnterLevel(-1);
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