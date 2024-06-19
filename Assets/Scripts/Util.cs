using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Util
{
    public static Vector3 ChangeX(Vector3 vec, float x)
    {
        vec.x = x;
        return vec;
    }

    public static Vector3 ChangeY(Vector3 vec, float y)
    {
        vec.y = y;
        return vec;
    }

    public static T ChooseRandom<T>(T[] array)
    {
        if (array == null || array.Length == 0)
        {
            Debug.LogWarning("Array is null or empty.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        return array[randomIndex];
    }
    public static T ChooseRandom<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("List is null or empty.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static void EnterLevel(int levelNum)
    {
        if (levelNum == -1)
        {
            SceneManager.LoadScene($"WinLost");
        }
        else if (levelNum == 0)
        {
            SceneManager.LoadScene($"ChooseLevel");
        }
        else
        {
            if (levelNum == 2)
            {
                SceneManager.LoadScene($"Level2Cutscene");
            }
            else
            {
                SceneManager.LoadScene($"Level{levelNum}");
            }
        }
    }

    public static int GetCurrentLevelNum()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level") && sceneName.Length > 5)
        {
            return int.Parse(sceneName[5..]);
        }
        else
        {
            Debug.Log($"Unrecognized scene name: {sceneName}");
            return 0;
        }
    }

    public static string GetNowTime()
    {
        return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

}


[Serializable]
public class Range
{
    public int Min;
    public int Max;

    public Range(int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException("Min should be less than or equal to Max.");
        }
        Min = min;
        Max = max;
    }

    public static Range operator *(Range range, float scalar)
    {
        int newMin = (int)Math.Round(range.Min * scalar);
        int newMax = (int)Math.Round(range.Max * scalar);
        if (newMin > newMax)
        {
            (newMax, newMin) = (newMin, newMax);
        }
        return new Range(newMin, newMax);
    }

    public int GetRandom()
    {
        return UnityEngine.Random.Range(Min, Max + 1);
    }

    public override string ToString()
    {
        return $"Range(Min: {Min}, Max: {Max})";
    }
}

