using UnityEngine;
using TMPro;

public class ManagerOffice : MonoBehaviour
{
    private string smoothieType = "2";

    public GameObject StrawberryRequest;
    public GameObject ChocolateRequest;

    public TMP_Text AddCoin;

    public TMP_Text TotalCoins;

    private int coinCount;
    private int orderAmount;
    void Start()
    {
        AddCoin.text = "+6";
        TotalCoins.text = " Coins";
        orderAmount = 6;
    }

    public bool Submit(string smoothie)
    {
        if (smoothie.Contains(smoothieType))
        {
            Request();
            return true;
        }
        return false;
    }

    private void Request()
    {
        coinCount += orderAmount;
        TotalCoins.text = coinCount + " Coins";
        smoothieType = Random.Range(0, 3).ToString();
        if (smoothieType.Contains("0"))
        {
            ChocolateRequest.SetActive(true);
            StrawberryRequest.SetActive(false);
            AddCoin.text = "+1";
            orderAmount = 1;
        }
        else if (smoothieType.Contains("1"))
        {
            StrawberryRequest.SetActive(true);
            ChocolateRequest.SetActive(false);
            AddCoin.text = "+2";
            orderAmount = 2;
        }
        else
        {
            StrawberryRequest.SetActive(true);
            ChocolateRequest.SetActive(true);
            AddCoin.text = "+6";
            orderAmount = 6;
        }
    }
}
