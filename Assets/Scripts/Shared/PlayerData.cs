using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BestScore
{
    public int score = 0;
    public bool present = false;
    public override string ToString()
    {
        return $"Best KPI: {score}";
    }
}

[System.Serializable]
public class PlayerData
{
    public List<BestScore> bestScores = new();

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(this);
        string path = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(path, json);
        Debug.Log($"Player data saved to {path}.");
    }

    public static PlayerData LoadPlayerData(int levelCount)
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
        data.AdjustToLevelCount(levelCount);
        data.SavePlayerData();
        return data;
    }

    private void AdjustToLevelCount(int levelCount)
    {
        for (int i = bestScores.Count; i < levelCount; i++)
        {
            bestScores.Add(new());
        }
    }
}

