using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public RequestManager requestManager;
    public IngredientData ingredientData;
    public Manager manager1;
    public Manager manager2;
    public Chest milkChest;
    public Chest strawberryChest;
    public SmoothieMachine smoothieMachine;
    public Clock clock;
    public GameObject minKpiText;
    public TutorialTextBox tutorialTextBox;
    public PlayerInteract playerInteract;
    public GameObject indicatorPrefab;
    private PlayerData playerData;
    private int phase = 0;
    private GameObject indicator1;
    private GameObject indicator2;

    void Start()
    {
        playerData = PlayerData.LoadPlayerData();
        if (!playerData.levelInfos[0].played)
        {
            manager1.TutorialManager = this;
            manager2.TutorialManager = this;
            playerInteract.disableDiscard = true;
            milkChest.Disable();
            strawberryChest.Disable();
            smoothieMachine.Disable();
            clock.gameObject.SetActive(false);
            tutorialTextBox.gameObject.SetActive(true);
            UpdatePhase();
        }
        else
        {
            tutorialTextBox.gameObject.SetActive(false);
        }
        tutorialTextBox.circularProgress.onCircularProgressDone = UpdatePhase;
    }

    void Update()
    {
        if (phase == 1)
        {
            if (IsHoldingIngredient("Milk"))
            {
                milkChest.Disable();
                UpdatePhase();
            }
        }
        else if (phase == 3)
        {
            if (indicator1 != null && IsHoldingIngredient("Milk"))
            {
                milkChest.Disable();
                Destroy(indicator1);
            }
            if (indicator2 != null && IsHoldingIngredient("Strawberry"))
            {
                strawberryChest.Disable();
                Destroy(indicator2);
            }
            if (IsHoldingIngredient("Milk") && IsHoldingIngredient("Strawberry"))
            {
                UpdatePhase();
            }
        }
        else if (phase == 4)
        {
            if (indicator1 != null & playerInteract.GetAllItems().Count < 2)
            {
                Destroy(indicator1);
                indicator2 = CreateIndicator(smoothieMachine.gameObject, new(0f, 2.2f, 0f));
            }
            if (indicator2 != null && playerInteract.GetAllItems().Count == 0)
            {
                Destroy(indicator2);
            }
            if (smoothieMachine.ProductCountdown <= 0f && playerInteract.GetAllItems().Count == 0)
            {
                UpdatePhase();
            }
        }
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
            tutorialTextBox.SetContents("Press C to grab a milk (use arrow keys to move).");
            var tutorialRequest = requestManager.GetTutorialRequest(new()
            {
                type = Item.Type.Ingredient,
                ingredients = new() { ingredientData.allIngredients[1] }
            });
            manager1.SetTutorialRequest(tutorialRequest);
            milkChest.Enable();

            indicator1 = CreateIndicator(milkChest.gameObject, new(0f, 1f, 0f));
            indicator2 = CreateIndicator(manager1.gameObject, new(0f, 1.8f, 0f));
        }
        else if (phase == 2)
        {
            Destroy(indicator1);
            tutorialTextBox.SetContents("Press C to give it to the manager.");
        }
        else if (phase == 3)
        {
            Destroy(indicator2);
            tutorialTextBox.SetContents("Nicely done! Now grab a milk and a strawberry.");
            var request = requestManager.GetTutorialRequest(new()
            {
                type = Item.Type.Smoothie,
                ingredients = new() { ingredientData.allIngredients[0], ingredientData.allIngredients[1] }
            });
            manager2.SetTutorialRequest(request);
            milkChest.Enable();
            strawberryChest.Enable();

            indicator1 = CreateIndicator(milkChest.gameObject, new(0f, 1f, 0f));
            indicator2 = CreateIndicator(strawberryChest.gameObject, new(0f, 1f, 0f));
        }
        else if (phase == 4)
        {
            tutorialTextBox.SetContents("Press C to put them in the smoothie machine.");
            smoothieMachine.Enable();

            indicator1 = CreateIndicator(smoothieMachine.gameObject, new(0f, 1f, 0f));
        }
        else if (phase == 5)
        {
            tutorialTextBox.SetContents("Press C to take out the smoothie, and give it to the manager.");

            indicator1 = CreateIndicator(manager2.gameObject, new(0f, 2.2f, 0f));
        }
        else if (phase == 6)
        {
            Destroy(indicator1);
            tutorialTextBox.SetContents("Well done! You may also press Q to throw away an item, or press R to switch between items.", true);
            playerInteract.disableDiscard = false;
        }
        else if (phase == 7)
        {
            tutorialTextBox.SetContents("Your goal is to earn enough KPI by 5PM. Now it's time to work!", true);

            indicator1 = CreateIndicator(minKpiText, new(-0.175f, -0.8f, 0f), true);
        }
        else if (phase == 8)
        {
            Destroy(indicator1);

            playerData.SavePlayerData();

            milkChest.Enable();
            strawberryChest.Enable();
            manager1.TutorialManager = null;
            manager2.TutorialManager = null;
            clock.gameObject.SetActive(true);
            tutorialTextBox.Hide();
            gameObject.SetActive(false);
            playerInteract.gameObject.GetComponent<PlayerControl>().waitForSpaceKeyUp = true;
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

    private bool IsHoldingIngredient(string ingredientName)
    {
        foreach (var item in playerInteract.GetAllItems())
        {
            if (item.type == Item.Type.Ingredient && item.ingredients[0].prefab.name.Contains(ingredientName))
            {
                return true;
            }
        }
        return false;
    }
}
