using UnityEngine;
using TMPro;
using System;

public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public float RewardMultiplier = 1f; // actual reward = rewardBase * rewardMultiplier (with fluctuations)
    public TMP_Text TotalCoins;
    private Request request;
    private GameObject requestDisplay;


    void Start()
    {
        request = requestManager.GetRequest(RewardMultiplier);
    }

    void Update()
    {
    }

    private void DisplayRequest()
    {
        
    }

    public bool Submit(string smoothie)
    {
        // if (smoothie.Contains(smoothieType))
        // {
        //     Request();
        //     return true;
        // }
        return false;
    }

    // private void Request()
    // {
    //     coinCount += orderAmount;
    //     TotalCoins.text = coinCount + " Coins";
    //     smoothieType = Random.Range(0, 3).ToString();
    //     if (smoothieType.Contains("0"))
    //     {
    //         ChocolateRequest.SetActive(true);
    //         StrawberryRequest.SetActive(false);
    //         orderAmount = 1;
    //     }
    //     else if (smoothieType.Contains("1"))
    //     {
    //         StrawberryRequest.SetActive(true);
    //         ChocolateRequest.SetActive(false);
    //         orderAmount = 2;
    //     }
    //     else
    //     {
    //         StrawberryRequest.SetActive(true);
    //         ChocolateRequest.SetActive(true);
    //         orderAmount = 6;
    //     }
    // }
}
