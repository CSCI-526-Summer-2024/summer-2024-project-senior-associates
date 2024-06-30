using UnityEngine;
using TMPro;

public class RequestManager : MonoBehaviour
{
    public GameObject requestBackgroundPrefab;
    public GameObject scorePrefab;
    public IngredientData ingredientData;
    public RequestProbability[] probList;
    public Range ItemRewardBase;
    public Range SingleSmoothieRewardBase;
    public Range DoubleSmoothieRewardBase;
    public int ItemMaxTime;
    public int SingleSmoothieMaxTime;
    public int DoubleSmoothieMaxTime;
    private int index = 0;
    private int numRequest = 0;
    public Vector3 scoreTextOffset = new(1.42f, -0.56f, 0f);
    private GameObject scoreText;

    public Request GetRequest(float rewardMultiplier)
    {
        if (numRequest >= probList[index].maxRequestNum)
        {
            return null;
        }

        var prob = GetProb();
        Request request = new();
        Range rewardBase;

        float randomValue = Random.value;
        if (randomValue < prob.itemProb)
        {
            request.item = new Item
            {
                type = Item.Type.Ingredient,
                ingredients = new() { Util.ChooseRandom(ingredientData.allIngredients) }
            };
            request.maxTime = ItemMaxTime;
            rewardBase = ItemRewardBase;
        }
        else if (randomValue < prob.itemProb + prob.singleItemSmoothieProb)
        {
            request.item = new Item
            {
                type = Item.Type.Smoothie,
                ingredients = ingredientData.GetSmoothieIngredients(1)
            };
            request.maxTime = SingleSmoothieMaxTime;
            rewardBase = SingleSmoothieRewardBase;
        }
        else
        {
            request.item = new Item
            {
                type = Item.Type.Smoothie,
                ingredients = ingredientData.GetSmoothieIngredients(2)
            };
            request.maxTime = DoubleSmoothieMaxTime;
            rewardBase = DoubleSmoothieRewardBase;
        }
        request.obj = CreateRequestObj(request);

        var rewardRange = rewardBase * rewardMultiplier;
        request.reward = rewardRange.GetRandom();
        Debug.Log($"rewardBase: {rewardBase}, rewardMultiplier: {rewardMultiplier}, rewardRange: {rewardRange}, chosen: {request.reward}");
        numRequest++;
        return request;
    }

    public Request GetTutorialRequest(Item item)
    {
        Request request = new()
        {
            item = item
        };

        Range rewardRange;
        if (item.type == Item.Type.Ingredient)
        {
            rewardRange = ItemRewardBase;
        }
        else
        {
            rewardRange = item.ingredients.Count == 2 ? SingleSmoothieRewardBase : DoubleSmoothieRewardBase;
        }
        request.reward = rewardRange.Min;
        request.obj = CreateRequestObj(request);

        return request;
    }

    private RequestProbability GetProb()
    {
        if (probList[index].remainingRequestNum <= 0 && index + 1 < probList.Length)
        {
            Debug.Log($"Finished sending all requests in #{index}, moving to #{index + 1}...");
            index++;
        }
        var requestProbability = probList[index];
        probList[index].remainingRequestNum--;

        return requestProbability;
    }

    public GameObject CreateRequestObj(Request request)
    {
        var obj = new GameObject("Request");
        obj.AddComponent<ShakeEffect>();
        var background = Instantiate(requestBackgroundPrefab);
        background.transform.SetParent(obj.transform);
        SetBackgroundPositionAndScale(request, background);

        GameObject item;
        if (request.item.type == Item.Type.Ingredient)
        {
            item = Instantiate(request.item.ingredients[0].prefab);
        }
        else
        {
            item = ingredientData.CreateSmoothieObj(request.item.ingredients);
        }
        item.transform.SetParent(obj.transform);
        SetItemPositionAndScale(request, item);

        GameObject score = Instantiate(scorePrefab);
        score.transform.SetParent(obj.transform);
        score.transform.localPosition = scoreTextOffset;
        score.transform.localScale = new(1.5f, 1.5f, 1f);

        return obj;
    }

    private void SetBackgroundPositionAndScale(Request request, GameObject background)
    {
        if (request.item.type == Item.Type.Ingredient)
        {
            background.transform.localPosition = new(0, 0.4f, 0f);
            background.transform.localScale = new(0.8f, 0.8f, 1f);
        }
        else
        {
            background.transform.localPosition = new(0, 0.5f, 0f);
            background.transform.localScale = new(0.6f * request.item.ingredients.Count, 1.2f, 1f);
        }
    }

    private void SetItemPositionAndScale(Request request, GameObject item)
    {
        if (request.item.type == Item.Type.Ingredient)
        {
            item.transform.localPosition = new(0, 0.4f, -0.1f);
        }
        else
        {
            item.transform.localPosition = new(0, 0.25f, -0.1f);
        }
    }

    public void FinishRequest()
    {
        numRequest--;
    }
}


[System.Serializable]
public class Request
{
    public GameObject obj;
    public Item item;
    public int reward;
    public int maxTime;
}

[System.Serializable]
public struct RequestProbability
{
    public float itemProb;
    public float singleItemSmoothieProb;
    public float doubleItemSmoothieProb;
    public int remainingRequestNum;
    public int maxRequestNum;

    public override readonly string ToString()
    {
        return $"RequestProb(Item: {itemProb}, SingleSmoothie: {singleItemSmoothieProb}, DoubleSmoothie: {doubleItemSmoothieProb})";
    }
}