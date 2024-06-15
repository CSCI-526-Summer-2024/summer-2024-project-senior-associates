using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{

    public CutsceneTextBox cutsceneTextBox;
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
        if (playerData.firstTimePlaying)
        {
            cutsceneTextBox.gameObject.SetActive(true);
            playerEnergy.enableEnergyDrop = false;
            UpdatePhase();
        }
        else
        {
            cutsceneTextBox.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (phase == 1)
        {

        }

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
            cutsceneTextBox.SetContents("Energy is tracked with Energy Bar", true);

            indicator1 = CreateIndicator(energyBar.gameObject, new(0.05f, -0.4f, 0f), true);
        }
        else if (phase == 2)
        {
            cutsceneTextBox.SetContents("Energy will drop", true);
            playerEnergy.enableEnergyDrop = true;
        }
        else if (phase == 3)
        {
            Destroy(indicator1);
            cutsceneTextBox.SetContents("Go to bed to refill Energy. Press space to start Level 2", true);
            indicator1 = CreateIndicator(bed.gameObject, new(-1f, 0.01f, 0));
        }
        else if (phase == 4)
        {
            Destroy(indicator1);
            /*
            playerData.firstTimePlaying = false;
            playerData.SavePlayerData();
            cutsceneTextBox.Hide();
            gameObject.SetActive(false);
            playerInteract.gameObject.GetComponent<PlayerControl>().waitForSpaceKeyUp = true;*/
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
