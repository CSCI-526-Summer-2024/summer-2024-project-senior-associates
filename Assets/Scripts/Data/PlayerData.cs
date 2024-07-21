using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo
{
    public int bestKpi = 0;
    public bool played = false;
    public bool unlocked = true;
    public int minKpi = 0;
    public string GetBestKpiString()
    {
        return $"Best KPI: {bestKpi}";
    }

    public void UpdateBestKpi(int newKpi)
    {
        if (!played || newKpi > bestKpi)
        {
            bestKpi = newKpi;
            played = true;
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public List<LevelInfo> levelInfos = new();

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(this);
        string path = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(path, json);
        Debug.Log($"Player data saved to {path}.");
    }

    public static PlayerData LoadPlayerData(int[] minKpis = null)
    {
        string path = Application.persistentDataPath + "/playerData.json";
        PlayerData data;
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Successfully read player data from {path}.");
        }
        else
        {
            data = new();
            Debug.Log($"Could not load player data from {path}, creating a new one.");
        }
        if (minKpis != null)
        {
            data.SetMinKpis(minKpis);
        }
        data.SavePlayerData();
        return data;
    }

    private void SetMinKpis(int[] minKpis)
    {
        for (int i = 0; i < minKpis.Length; i++)
        {
            if (i >= levelInfos.Count)
            {
                levelInfos.Add(new());
            }
            levelInfos[i].minKpi = minKpis[i];
        }
    }
}

