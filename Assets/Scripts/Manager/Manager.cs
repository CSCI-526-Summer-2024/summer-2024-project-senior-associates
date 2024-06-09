using UnityEngine;
using TMPro;
using System;

public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public Range RequestCountdownStartingRange;
    public float RewardMultiplier; // actual reward = rewardBase * rewardMultiplier (with fluctuations)
    public TMP_Text TotalCoins;

    private readonly Vector3 ManagerToRequestOffset = new(0f, 0.5f, 0.5f);
    private Request request;
    private float nextRequestCountdown = 0.1f;

    void Update()
    {
        if (nextRequestCountdown > 0f)
        {
            nextRequestCountdown -= Time.deltaTime;
            if (nextRequestCountdown <= 0f)
            {
                request = requestManager.GetRequest(RewardMultiplier);
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
            Destroy(request.obj);
            request = null;
            nextRequestCountdown = RequestCountdownStartingRange.GetRandom();
            Debug.Log($"Next request countdown: {nextRequestCountdown}");
            //update Mood bar
            GetComponent<ManagerMood>().updateMood();
        }
        else
        {
            Debug.Log("Player is trying to give the wrong item.");
        }
        return satisfied;
    }
}
