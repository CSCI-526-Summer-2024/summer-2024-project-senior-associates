using UnityEngine;
using System;


public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public UIManager uiManager;
    public Range RequestCountdownStartingRange;
    private TutorialManager tutorialManager; public bool InTutorial => tutorialManager != null;
    private readonly float RewardMultiplier = 1f; // actual reward = rewardBase * rewardMultiplier (with fluctuations)
    private readonly Vector3 ManagerToRequestOffset = new(0f, 0.5f, 0.5f);
    private Request request = null; public Request Request => request;
    private float nextRequestCountdown = 0f;
    private ManagerMood mood;
    private bool disabled = false;

    void Awake()
    {
        mood = GetComponent<ManagerMood>();
        mood.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!InTutorial && !disabled)
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
                    PositionRequest();
                    mood.SetRequestMaxTime(request.maxTime);
                }
            }
        }
    }

    public void SetTutorialRequest(Request tutorialRequest)
    {
        request = tutorialRequest;
        PositionRequest();
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
            request.obj.GetComponent<ShakeEffect>().Trigger(0.5f, 0.02f);
        }
        return satisfied;
    }

    public void Schmooze()
    {
        uiManager.UpdateScore(CalculateReward(request.reward, mood.Mood), transform.position);
        FinishRequest();
    }

    public void FinishRequest()
    {
        mood.Reset();
        Destroy(request?.obj);
        request = null;

        if (InTutorial)
        {
            tutorialManager.OnRequestSatisfied();
        }
        else
        {
            nextRequestCountdown = RequestCountdownStartingRange.GetRandom();
            requestManager.FinishRequest();
        }
    }

    public void Disable()
    {
        disabled = true;
        FinishRequest();
    }

    public TutorialManager TutorialManager
    {
        set { tutorialManager = value; }
    }

    private int CalculateReward(int reward, float mood)
    {
        return InTutorial ? reward : (int)Math.Round(reward * mood * 2);
    }

    private void PositionRequest()
    {
        request.obj.transform.SetParent(transform);
        request.obj.transform.localPosition = ManagerToRequestOffset;
    }

}