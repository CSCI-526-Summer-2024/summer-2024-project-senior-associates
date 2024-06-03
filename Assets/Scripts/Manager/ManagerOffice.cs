using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerOffice : MonoBehaviour
{
    private string smoothieType="2";

    public GameObject StrawberryRequest;
    public GameObject ChocolateRequest;
    // Update is called once per frame
    void Start()
    {
        StrawberryRequest=GameObject.Find("StrawberryRequest");
        ChocolateRequest=GameObject.Find("ChocolateRequest");
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

        this.smoothieType= Random.Range(0, 3).ToString();
        if(smoothieType.Contains("0")){
            StrawberryRequest.SetActive(true);
            StrawberryRequest.SetActive(false);
        }else if(smoothieType.Contains("1")){
            StrawberryRequest.SetActive(true);
            ChocolateRequest.SetActive(false);
        }else{
            StrawberryRequest.SetActive(true);
            ChocolateRequest.SetActive(true);
        }
    }
}
