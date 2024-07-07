using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public enum SubmitResult
{
    Unsubmittable,
    Failed,
    SubmittedRight,
    SubmittedLeft,
}

public class Manager : MonoBehaviour
{
    public RequestManager requestManager;
    public UIManager uiManager;
    public Range RequestCountdownStartingRange;
    public GameObject schmoozeTextPrefab;
    public Vector3 schmoozeTextOffset = new(1.5f, 0.5f, 0f);
    private TutorialManager tutorialManager; public bool InTutorial => tutorialManager != null;
    private readonly float RewardMultiplier = 1f; // actual reward = rewardBase * rewardMultiplier (with fluctuations)
    private readonly Vector3 ManagerToRequestOffset = new(0f, 0.5f, 0.5f);
    private Request request = null; public Request Request => request;
    private float nextRequestCountdown = 0f;
    private ManagerMood mood;
    private bool disabled = false;
    private PlayerEnergy playerEnergy;
    private GameObject schmoozeText;

    void Awake()
    {
        mood = GetComponent<ManagerMood>();
        mood.gameObject.SetActive(true);
        playerEnergy = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEnergy>();
        if (schmoozeTextPrefab != null)
        {
            schmoozeText = Instantiate(schmoozeTextPrefab);
            schmoozeText.transform.SetParent(transform);
            schmoozeText.transform.localPosition = schmoozeTextOffset;
            schmoozeText.SetActive(false);
        }
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
            if (playerEnergy != null && playerEnergy.CanSchmooze() && request != null)
            {
                ShowSchmoozeText();
            }
            else
            {
                HideSchmoozeText();
            }
        }
        if (request != null)
        {
            request.obj.GetComponentInChildren<TextMeshProUGUI>().text = "+ " + CalculateReward(request.reward, mood.Mood).ToString() + " KPI";

        }
    }

    public void SetTutorialRequest(Request tutorialRequest)
    {
        request = tutorialRequest;
        PositionRequest();
    }

    public SubmitResult TrySubmit(List<Item> items)
    {
        if (request == null || items.Count == 0)
        {
            return SubmitResult.Unsubmittable;
        }
        for (var idx = 0; idx < items.Count && idx < 2; idx++)
        {
            if (items[idx] == request.item)
            {
                return idx == 0 ? SubmitResult.SubmittedLeft : SubmitResult.SubmittedRight;
            }
        }
        return SubmitResult.Failed;
    }

    public SubmitResult Submit(List<Item> items)
    {
        var res = TrySubmit(items);
        if (res == SubmitResult.Failed)
        {
            request.obj.GetComponent<ShakeEffect>().Trigger(0.5f, 0.02f);
            uiManager.AddWrongItemNum();
        }
        else if (res == SubmitResult.SubmittedLeft || res == SubmitResult.SubmittedRight)
        {
            uiManager.UpdateScore(false, CalculateReward(request.reward, mood.Mood), transform.position);
            FinishRequest();
        }
        return res;
    }

    public void Schmooze()
    {
        uiManager.UpdateScore(true, CalculateReward(request.reward, mood.Mood) / 2, transform.position);
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

    private void ShowSchmoozeText()
    {
        if (schmoozeText != null && !schmoozeText.activeSelf)
        {
            schmoozeText.SetActive(true);
            schmoozeText.GetComponentInChildren<FlickerEffect>().Trigger(0.6f, 0.15f);
        }
    }

    private void HideSchmoozeText()
    {
        if (schmoozeText != null)
        {
            schmoozeText.SetActive(false);
        }
    }

}
