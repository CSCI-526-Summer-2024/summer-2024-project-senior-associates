using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public IngredientData ingredientData;
    public RequestProbability[] probList;
    public Range ItemRewardBase;
    public Range SingleSmoothieRewardBase;
    public Range DoubleSmoothieRewardBase;
    private int index = 0;

    public Request GetRequest(float rewardMultiplier)
    {
        var prob = GetProb();
        Debug.Log($"Prob: {prob}");
        Request request = new();
        Range rewardBase;

        float randomValue = UnityEngine.Random.value;
        if (randomValue < prob.itemProb)
        {
            request.type = Request.Type.Item;
            request.ingredients = new() { Util.ChooseRandom(ingredientData.allIngredients) };
            rewardBase = ItemRewardBase;
        }
        else if (randomValue < prob.itemProb + prob.singleItemSmoothieProb)
        {
            request.type = Request.Type.Smoothie;
            request.ingredients = ingredientData.GetSmoothieIngredients(1);
            rewardBase = SingleSmoothieRewardBase;
        }
        else
        {
            request.type = Request.Type.Smoothie;
            request.ingredients = ingredientData.GetSmoothieIngredients(2);
            rewardBase = DoubleSmoothieRewardBase;
        }
        var rewardRange = rewardBase * rewardMultiplier;
        request.maxReward = UnityEngine.Random.Range(rewardRange.Min, rewardRange.Max + 1);
        Debug.Log($"rewardBase: {rewardBase}, rewardMultiplier: {rewardMultiplier}, rewardRange: {rewardRange}, chosen: {request.maxReward}");

        return request;
    }

    private RequestProbability GetProb()
    {
        var requestProbability = probList[index];
        probList[index].numRequestsLeft--;
        if (probList[index].numRequestsLeft <= 0 && index + 1 < probList.Length)
        {
            Debug.Log($"Finished sending all requests in #{index}, moving to #{index + 1}...");
            index++;
        }
        return requestProbability;
    }
}


[System.Serializable]
public class Request
{
    public enum Type { Item, Smoothie }
    public Type type;
    public List<Ingredient> ingredients;
    public int maxReward;
}

[System.Serializable]
public struct RequestProbability
{
    public float itemProb;
    public float singleItemSmoothieProb;
    public float doubleItemSmoothieProb;
    public int numRequestsLeft;

    public override string ToString()
    {
        return $"RequestProb(Item: {itemProb}, SingleSmoothie: {singleItemSmoothieProb}, DoubleSmoothie: {doubleItemSmoothieProb})";
    }
}