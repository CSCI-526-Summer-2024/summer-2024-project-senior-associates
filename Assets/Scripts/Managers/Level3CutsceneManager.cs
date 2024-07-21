using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3CutsceneManager : MonoBehaviour
{
    public RequestManager requestManager;
    public Manager manager;
    public TutorialTextBox tutorialTextBox;
    public PlayerInteract playerInteract;
    public GameObject indicatorPrefab;
    public GameObject energyBar;
    public GameObject schmoozeIndicator;
    private PlayerData playerData;
    private int phase = 0;
    private GameObject indicator1;
    private IngredientData IngredientData => requestManager.ingredientData;

    void Start()
    {
        playerData = PlayerData.LoadPlayerData();
        if (!playerData.levelInfos[2].played)
        {
            manager.level3CutsceneManager = this;
            tutorialTextBox.gameObject.SetActive(true);
            UpdatePhase();
        }
        else
        {
            tutorialTextBox.gameObject.SetActive(false);
            SceneManager.LoadScene("Level3");
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
            tutorialTextBox.SetContents("When you are near the manager, you could press T to schmooze them...");
            var tutorialRequest = requestManager.GetTutorialRequest(new()
            {
                type = Item.Type.Smoothie,
                ingredients = new() { IngredientData.allIngredients[0], IngredientData.allIngredients[1], IngredientData.allIngredients[2] }
            });
            manager.SetTutorialRequest(tutorialRequest);
            indicator1 = CreateIndicator(manager.gameObject, new(0f, 2.2f, 0f));
        }
        else if (phase == 2)
        {
            Destroy(indicator1);
            tutorialTextBox.SetContents("Nicely schmoozed! Now the manager has forgotten their request, and you earned a little KPI.", true);
        }
        else if (phase == 3)
        {
            indicator1 = CreateIndicator(schmoozeIndicator, new(1.6f, -0.6f, 0f), true);
            tutorialTextBox.SetContents("You can Schmooze when this T is blue again or if this icon is fully loaded", true);
        }
        else if (phase == 4)
        {
            Destroy(indicator1);
            tutorialTextBox.SetContents("Be careful! Schmoozing costs a lot of energy...", true);
            indicator1 = CreateIndicator(energyBar, new(0.05f, -0.4f, 0f), true);
        }
        else if (phase == 5)
        {
            Destroy(indicator1);
            playerData.SavePlayerData();
            DataManager.waitForSpaceKeyUpAtStart = true;
            SceneManager.LoadScene("Level3");
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
