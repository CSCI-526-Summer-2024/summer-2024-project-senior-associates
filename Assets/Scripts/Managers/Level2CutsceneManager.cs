using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2CutsceneManager : MonoBehaviour
{
    public TutorialTextBox tutorialTextBox;
    public PlayerInteract playerInteract;
    public PlayerEnergy playerEnergy;
    public GameObject indicatorPrefab;
    public GameObject energyBar;
    public GameObject bed;
    private PlayerData playerData;
    private int phase = 0;
    private GameObject indicator1;

    void Start()
    {
        playerData = PlayerData.LoadPlayerData();
        if (!playerData.levelInfos[1].played)
        {
            tutorialTextBox.gameObject.SetActive(true);
            playerEnergy.enableEnergyDrop = false;
            UpdatePhase();
        }
        else
        {
            tutorialTextBox.gameObject.SetActive(false);
            SceneManager.LoadScene("Level2");
        }
        tutorialTextBox.circularProgress.onCircularProgressDone = UpdatePhase;
    }

    public void OnCircularProgressDone()
    {
        UpdatePhase();
    }

    public void OnRequestSatisfied()
    {
        UpdatePhase();
    }

    private void UpdatePhase()
    {
        phase++;
        if (phase == 1)
        {
            tutorialTextBox.SetContents("From now on, you need energy to move around.", true);
            indicator1 = CreateIndicator(energyBar, new(0.05f, -0.4f, 0f), true);
        }
        else if (phase == 2)
        {
            tutorialTextBox.SetContents("Energy will drop as you move...", true);
            playerEnergy.enableEnergyDrop = true;
        }
        else if (phase == 3)
        {
            Destroy(indicator1);
            tutorialTextBox.SetContents("Go to bed to restore energy before it depletes!", true);
            indicator1 = CreateIndicator(bed, new(-1f, 0.5f, 0));
        }
        else if (phase == 4)
        {
            Destroy(indicator1);
            playerData.SavePlayerData();
            DataManager.waitForSpaceKeyUpAtStart = true;
            SceneManager.LoadScene($"Level2");
        }
    }

    private GameObject CreateIndicator(GameObject obj, Vector3 offset, bool upSideDown = false)
    {
        var indicator = Instantiate(indicatorPrefab);
        if (upSideDown)
        {
            indicator.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        indicator.GetComponent<FloatingAnim>().Init(obj, offset);
        return indicator;
    }

}
