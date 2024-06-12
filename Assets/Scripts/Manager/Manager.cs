using UnityEngine;
using System;


public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public UIManager uiManager;
    public Range RequestCountdownStartingRange;
    public float RewardMultiplier = 1f; // actual reward = rewardBase * rewardMultiplier (with fluctuations)
    private readonly Vector3 ManagerToRequestOffset = new(0f, 0.5f, 0.5f);
    private Request request = null; public Request Request => request;
    private float nextRequestCountdown = 0.1f;
    private ManagerMood mood;

    void Start()
    {
        mood = GetComponent<ManagerMood>();
    }

    void Update()
    {
        if (nextRequestCountdown > 0f)
        {
            nextRequestCountdown -= Time.deltaTime;
        }
        if (request == null && nextRequestCountdown <= 0f)
        {
            request = requestManager.GetRequest(RewardMultiplier);
            if (request != null)
            {
                request.obj.transform.SetParent(transform);
                request.obj.transform.localPosition = ManagerToRequestOffset;
                mood.SetRequestMaxTime(request.maxTime);
            }
        }
    }

    public bool Submit(Item item)
    {
        if (request == null || item == null)
        {
            return false;
        }

        var satisfied = request.item == item;
        if (satisfied)
        {
            uiManager.UpdateScore(CalculateReward(request.reward, mood.Mood), transform.position);
            FinishRequest();
        }
        else
        {
            Debug.Log("Player is trying to give the wrong item.");
        }
        return satisfied;
    }

    public void Schmooze()
    {
        // request satisfied + KPI
        uiManager.UpdateScore(CalculateReward(request.reward, mood.Mood), transform.position);
        FinishRequest();
    }

    public void FinishRequest()
    {
        nextRequestCountdown = RequestCountdownStartingRange.GetRandom();
        mood.Reset();
        Destroy(request.obj);
        request = null;
        requestManager.FinishRequest();
    }

    private int CalculateReward(int reward, float mood)
    {
        return (int)Math.Round(reward * mood * 2);
    }
}
