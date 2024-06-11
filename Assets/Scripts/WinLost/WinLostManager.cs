using TMPro;
using UnityEngine;


public class WinLostManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text minKpiText;
    public TMP_Text deliveredNumText;
    public TMP_Text deliveredKpiText;
    public TMP_Text failedNumText;
    public TMP_Text failedKpiText;
    public TMP_Text totalKpiText;

    void Start()
    {
        SetText(PlayerPrefs.GetInt("MinKpi", 0), PlayerPrefs.GetInt("DeliveredNum", 0), PlayerPrefs.GetInt("DeliveredKPI", 0), PlayerPrefs.GetInt("FailedNum", 0), PlayerPrefs.GetInt("FailedKPI", 0));
    }

    private void SetText(int minKpi, int deliveredNum, int deliveredKPI, int failedNum, int failedKPI)
    {
        var totalKpi = deliveredKPI + failedKPI;
        if (totalKpi >= minKpi)
        {
            titleText.text = "Level Cleared";
            titleText.color = Color.green;
        }
        else
        {
            titleText.text = "Level Lost";
            titleText.color = Color.red;
        }
        minKpiText.text = $"Minimum KPI to pass level: {minKpi}";
        deliveredNumText.text = $"Requests Delivered x {deliveredNum}";
        deliveredKpiText.text = $"{deliveredKPI}";
        failedNumText.text = $"Requests Failed x {failedNum}";
        failedKpiText.text = $"{failedKPI}";
        totalKpiText.text = $"{totalKpi}";
    }
}
