using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;




public class LevelData
{
    public string date;
    public int level;
    public int deliveredNum = 0;
    public int deliveredKpi = 0;
    public int failedNum = 0;
    public int failedKpi = 0;
    public int wrongItemNum = 0;
    public int schmoozeNum = 0;
    public int schmoozeKpi = 0;
    public List<int> kpiTrend;

    public LevelData()
    {
        kpiTrend = new List<int>();
    }

}

public class UIManager : MonoBehaviour
{
    public GameObject gainedScoreAboveManagerPrefab;
    public GameObject timeUpNoticePrefab;
    public CurrentScoreText currentScoreText;
    public Canvas canvas;
    public TMP_Text minKpiText;
    private PlayerData playerData;
    private readonly float LevelEndPauseDuration = 3f;
    private int levelNum;
    private float levelEndPauseCountdown = 0f;

    void Start()
    {
        levelNum = Util.GetCurrentLevelNum() - 1;
        playerData = PlayerData.LoadPlayerData();
        minKpiText.text = $"Goal: {playerData.levelInfos[levelNum].minKpi}";

        InvokeRepeating(nameof(RecordKPI), 0f, 5f);
    }


    private void RecordKPI()
    {
        DataManager.levelDataFirebase.kpiTrend.Add(DataManager.levelDataFirebase.deliveredKpi + DataManager.levelDataFirebase.failedKpi + DataManager.levelDataFirebase.schmoozeKpi);
        //Debug.Log("kpiTrend : " + DataManager.levelDataFirebase.kpiTrend);
    }


    void Update()
    {
        if (levelEndPauseCountdown > 0f)
        {
            levelEndPauseCountdown -= Time.deltaTime;
            if (levelEndPauseCountdown <= 0f)
            {
                Util.EnterLevel(-1);
            }
        }
    }


    public void UpdateScore(Boolean isSchmooze, int score, Vector3 position)
    {
        if (isSchmooze == true)
        {
            DataManager.levelDataFirebase.schmoozeNum++;
            DataManager.levelDataFirebase.schmoozeKpi += score;
            Debug.Log("schmoozeNum: " + DataManager.levelDataFirebase.schmoozeNum + "   schmoozeKpi: " + DataManager.levelDataFirebase.schmoozeKpi);
        }
        else
        {
            if (score > 0)
            {
                DataManager.levelDataFirebase.deliveredNum++;
                DataManager.levelDataFirebase.deliveredKpi += score;
                Debug.Log("Submitted: " + DataManager.levelDataFirebase.deliveredNum);
            }
            else
            {
                DataManager.levelDataFirebase.failedNum++;
                DataManager.levelDataFirebase.failedKpi += score;
            }
        }

        currentScoreText.UpdateScore(score);
        CreateGainedScoreText(score, position + new Vector3(0, 0.5f, -1f));
        CreateGainedScoreText(score, currentScoreText.GetBottomRightPoint() + new Vector3(1f, -0.2f, -1f));
    }

    public void AddWrongItemNum()
    {
        DataManager.levelDataFirebase.wrongItemNum++;
    }

    public void EndLevel()
    {
        levelEndPauseCountdown = LevelEndPauseDuration;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().disableAllAction = true;
        foreach (var manager in GameObject.FindGameObjectsWithTag("Manager"))
        {
            manager.GetComponent<Manager>().Disable();
        }
        Instantiate(timeUpNoticePrefab);

        var totalKpi = DataManager.levelDataFirebase.deliveredKpi + DataManager.levelDataFirebase.failedKpi + DataManager.levelDataFirebase.schmoozeKpi;
        Debug.Log("UI manager endlevel " + totalKpi);
        playerData.levelInfos[levelNum].UpdateBestKpi(totalKpi);
        if (levelNum + 1 < playerData.levelInfos.Count && totalKpi >= playerData.levelInfos[levelNum].minKpi)
        {
            playerData.levelInfos[levelNum + 1].unlocked = true;
        }
        playerData.SavePlayerData();

        DataManager.levelDataFirebase.date = Util.GetNowTime();
        DataManager.levelDataFirebase.level = Util.GetCurrentLevelNum();

        PlayerPrefs.SetInt("MinKpi", playerData.levelInfos[levelNum].minKpi);
        PlayerPrefs.SetInt("DeliveredNum", DataManager.levelDataFirebase.deliveredNum + DataManager.levelDataFirebase.schmoozeNum);
        PlayerPrefs.SetInt("DeliveredKPI", DataManager.levelDataFirebase.deliveredKpi + DataManager.levelDataFirebase.schmoozeKpi);
        PlayerPrefs.SetInt("FailedNum", DataManager.levelDataFirebase.failedNum);
        PlayerPrefs.SetInt("FailedKPI", DataManager.levelDataFirebase.failedKpi);
        PlayerPrefs.SetInt("WrongItemNum", DataManager.levelDataFirebase.wrongItemNum);
        PlayerPrefs.SetString("Date", Util.GetNowTime());
        PlayerPrefs.SetInt("Level", Util.GetCurrentLevelNum());



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