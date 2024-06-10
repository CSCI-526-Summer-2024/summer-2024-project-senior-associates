using UnityEngine;
using TMPro;
using System;

public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public UIManager uiManager;
    public Range RequestCountdownStartingRange;
    public float RewardMultiplier; // actual reward = rewardBase * rewardMultiplier (with fluctuations)

    private readonly Vector3 ManagerToRequestOffset = new(0f, 0.5f, 0.5f);
    private Request request = null;
    private float nextRequestCountdown = 0.1f;
    private ManagerMood mood;
    public Request Request => request;

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
            uiManager.UpdateScore((int)Math.Round(request.reward * mood.Mood), transform.position);
            FinishRequest();
            Debug.Log($"Next request countdown: {nextRequestCountdown}");
        }
        else
        {
            Debug.Log("Player is trying to give the wrong item.");
        }
        return satisfied;
    }

    public void FinishRequest()
    {
        nextRequestCountdown = RequestCountdownStartingRange.GetRandom();
        mood.Reset();
        Destroy(request.obj);
        request = null;
        requestManager.FinishRequest();
    }
}
