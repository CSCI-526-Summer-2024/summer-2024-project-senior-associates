using UnityEngine;

public class TutorialManagerLevel2 : TutorialManager
{
    public PlayerEnergy playerEnergy;
    public PlayerInteract playerInteract;
    public RequestManager reqManager;
    public Chest milkCh;
    public Chest strawCh;
    public Chest waterCh;
    public SmoothieMachine smoothieMach;
    public Clock gameClock;
    public GameObject energyBar;
    public GameObject minimumKpiText;
    public TutorialTextBox tutTextBox;
    public GameObject indicatPrefab;
    public GameObject man1;
    public GameObject man2;
    public GameObject managerMood;
    public GameObject smoothieProgress;

    public GameObject bed;
    private PlayerData playData;
    private int phaseNum = 0;
    private GameObject indicator;

    void Start()
    {
        playData = PlayerData.LoadPlayerData();
        //Debug.Log(playData.levelInfos);
        if (playData.firstTimePlaying)
        {
            playerEnergy.enableEnergyDrop = false;

            milkCh.gameObject.SetActive(false);
            strawCh.gameObject.SetActive(false);
            waterCh.gameObject.SetActive(false);
            smoothieMach.gameObject.SetActive(false);

            reqManager.gameObject.SetActive(false);
            man1.gameObject.SetActive(false);
            man2.gameObject.SetActive(false);
            managerMood.gameObject.SetActive(false);
            smoothieProgress.gameObject.SetActive(false);
            gameClock.gameObject.SetActive(false);
            tutTextBox.gameObject.SetActive(true);
            UpdatePhase();
        }
        else
        {
            tutTextBox.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (phaseNum == 3)
        {
            if (playerEnergy.IsSleeping)
            {
                UpdatePhase();
            }
        }
        else if (phaseNum == 4)
        {
            if (!playerEnergy.IsSleeping)
            {
                UpdatePhase();
            }
        }

    }

    public override void OnCircularProgressDone()
    {

        UpdatePhase();
    }

    public override void OnRequestSatisfied()
    {
        UpdatePhase();
    }

    public override void UpdatePhase()
    {
        phaseNum++;
        if (phaseNum == 1)
        {
            tutTextBox.SetContents("This is energy bar", true);
            indicator = CreateIndicator(energyBar.gameObject, new(0.05f, -0.4f, 0f), true);
        }
        else if (phaseNum == 2)
        {
            tutTextBox.SetContents("Energy will drop over time", true);
            playerEnergy.enableEnergyDrop = true;
        }
        else if (phaseNum == 3)
        {
            Destroy(indicator);
            tutTextBox.SetContents("Go to bed and press C");
            indicator = CreateIndicator(bed.gameObject, new(0f, 0.5f, 0f));
        }
        else if (phaseNum == 4)
        {
            Destroy(indicator);
            tutTextBox.SetContents("Press C to wake up and Keep Track of your Energy");
        }
        else if (phaseNum == 5)
        {

            playData.firstTimePlaying = false;
            playData.SavePlayerData();

            milkCh.gameObject.SetActive(true);
            strawCh.gameObject.SetActive(true);
            waterCh.gameObject.SetActive(true);
            smoothieMach.gameObject.SetActive(true);

            reqManager.gameObject.SetActive(true);
            managerMood.gameObject.SetActive(true);
            man1.gameObject.SetActive(true);
            man2.gameObject.SetActive(true);
            smoothieProgress.gameObject.SetActive(true);
            gameClock.gameObject.SetActive(true);
            tutTextBox.Hide();
        }
    }

    private GameObject CreateIndicator(GameObject obj, Vector3 offset, bool upSideDown = false)
    {
        var indicator = Instantiate(indicatPrefab);
        if (upSideDown)
        {
            indicator.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        indicator.GetComponent<FloatingAnim>().Init(obj, offset);
        return indicator;
    }


}
