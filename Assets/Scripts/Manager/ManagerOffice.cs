using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerOffice : MonoBehaviour
{
    private string smoothieType="2";

    public GameObject StrawberryRequest;
    public GameObject ChocolateRequest;

    private TMP_Text AddCoin;

    private TMP_Text TotalCoins;

    private int coinCount;
    private int orderAmount;
    void Start()
    {
        StrawberryRequest=GameObject.Find("StrawberryRequest");
        ChocolateRequest=GameObject.Find("ChocolateRequest");
        AddCoin=GameObject.Find("AddCoin").GetComponent<TMP_Text>();
        AddCoin.text="+6";
        TotalCoins=GameObject.Find("TotalCoins").GetComponent<TMP_Text>();
        TotalCoins.text=" Coins";
        orderAmount=6;

    }
    void Update()
    {
       
    }

    public bool submit(string smoothie)
    {
        if(smoothie.Contains(smoothieType))
        {
            request();
            return true;
        }
        return false;
    }

    private void request()
    {
        coinCount+=orderAmount;
        TotalCoins.text=coinCount+ " Coins";
        this.smoothieType= Random.Range(0, 3).ToString();
        if(smoothieType.Contains("0")){
            ChocolateRequest.SetActive(true);
            StrawberryRequest.SetActive(false);
            AddCoin.text="+1";
            orderAmount=1;
        }else if(smoothieType.Contains("1")){
            StrawberryRequest.SetActive(true);
            ChocolateRequest.SetActive(false);
            AddCoin.text="+2";
            orderAmount=2;
        }else{
            StrawberryRequest.SetActive(true);
            ChocolateRequest.SetActive(true);
            AddCoin.text="+6";
            orderAmount=6;
        }
    }
}
