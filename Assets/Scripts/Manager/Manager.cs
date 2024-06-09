using UnityEngine;
using TMPro;
using System;
using UnityEngine.Assertions;

public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public UIManager uiManager;
    public Range RequestCountdownStartingRange;
    public float RewardMultiplier; // actual reward = rewardBase * rewardMultiplier (with fluctuations)
    public TMP_Text TotalCoins;

    private readonly Vector3 ManagerToRequestOffset = new(0f, 0.5f, 0.5f);
    private Request request;
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
            nextRequestCountdown = RequestCountdownStartingRange.GetRandom();
            Debug.Log($"Next request countdown: {nextRequestCountdown}");
            uiManager.AddScore((int)Math.Round(request.reward * mood.Value), transform.position);
            mood.updateMood();

            Destroy(request.obj);
            request = null;
        }
        else
        {
            Debug.Log("Player is trying to give the wrong item.");
        }
        return satisfied;
    }
}
