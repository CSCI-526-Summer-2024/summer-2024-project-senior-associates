using UnityEngine;
using System;


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
    }

    public void SetTutorialRequest(Request tutorialRequest)
    {
        request = tutorialRequest;
        PositionRequest();
    }

    public bool Submit(Item item, bool peek)
    {
        if (request == null || item == null)
        {
            return false;
        }

        var satisfied = request.item == item;
        if (peek)
        {
            return satisfied;
        }
        if (satisfied)
        {

            uiManager.UpdateScore(CalculateReward(request.reward, mood.Mood), transform.position);
            FinishRequest();

        }
        else
        {
            request.obj.GetComponent<ShakeEffect>().Trigger(0.5f, 0.02f);
            uiManager.AddWrongItemNum();
        }
        return satisfied;
    }

    public void Schmooze()
    {
        uiManager.UpdateScore(CalculateReward(request.reward, mood.Mood) / 2, transform.position);
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
