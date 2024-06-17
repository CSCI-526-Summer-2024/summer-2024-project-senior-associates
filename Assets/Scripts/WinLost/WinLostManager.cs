using TMPro;
using UnityEngine;
using Proyecto26;

public class FirebaseData
{
    public int deliveredNum;
    public int failedNum;
    public int totalRequestNum;
    public int wrongItemNum;
}


public class WinLostManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text minKpiText;
    public TMP_Text deliveredNumText;
    public TMP_Text deliveredKpiText;
    public TMP_Text failedNumText;
    public TMP_Text failedKpiText;
    public TMP_Text totalKpiText;

    public int DeliveredNum;
    public int FailedNum;
    public int TotalRequestNum;
    public int WrongItemNum;

    FirebaseData firebaseData = new();

    void Start()
    {
        SetText(PlayerPrefs.GetInt("MinKpi", 0), PlayerPrefs.GetInt("DeliveredNum", 0), PlayerPrefs.GetInt("DeliveredKPI", 0), PlayerPrefs.GetInt("FailedNum", 0), PlayerPrefs.GetInt("FailedKPI", 0));
        SendDataFirebase();
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


    private void SendDataFirebase()
    {

        DeliveredNum = PlayerPrefs.GetInt("DeliveredNum", 0);
        FailedNum = PlayerPrefs.GetInt("FailedNum", 0);
        TotalRequestNum = PlayerPrefs.GetInt("TotalRequestNum", 0);
        WrongItemNum = PlayerPrefs.GetInt("WrongItemNum", 0);

        firebaseData.deliveredNum = DeliveredNum;
        firebaseData.failedNum = FailedNum;
        firebaseData.totalRequestNum = TotalRequestNum;
        firebaseData.wrongItemNum = WrongItemNum;

        RestClient.Post("https://cs526-senior-associates-default-rtdb.firebaseio.com/data.json", firebaseData);

    }

}
