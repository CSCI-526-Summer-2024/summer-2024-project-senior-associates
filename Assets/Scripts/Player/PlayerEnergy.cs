using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEnergy : MonoBehaviour
{
    public GameObject energyBar;
    public GameObject bed;
    public GameObject cPrefab;
    public GameObject bedText;
    public Clock clock;
    public bool enableEnergyDrop = true;
    public TextMeshProUGUI schmoozeCommand;
    public GameObject schmoozeIndicator;
    private readonly float NormalEnergyChange = -0.013f;
    private readonly float SleepEnergyChange = 0.15f;
    private readonly float SchmoozeEnergyDrop = 0.3f;
    private readonly Color NoSchmoozeColor = Color.gray;
    private readonly Color SchmoozeColor = Color.cyan;
    private readonly Color ZeroEnergyColor = Color.gray;
    private readonly Color FullEnergyColor = Color.cyan;
    private readonly Color IndicatorGreyedColor = new Color(85f / 255f, 85f / 255f, 85f, 255f);
    private readonly float MinEnergy = 0.1f;
    private float energyChange;
    private float energy;
    private bool isSleeping = false; public bool IsSleeping => isSleeping;
    private float energyBarOriginalXScale;
    private GameObject indicator;
    private int levelNum;
    private bool createdIndicator = false;
    private int schmoozeHour = 0;
    private bool shakeCheck = true;

    void Start()
    {
        energy = 1f;
        energyChange = NormalEnergyChange;
        energyBarOriginalXScale = energyBar.transform.localScale.x;
        levelNum = Util.GetCurrentLevelNum();
        if (levelNum == 3)
        {
            energyChange /= 2;
        }
        if (bedText != null)
        {
            bedText.SetActive(false);
        }
    }

    void Update()
    {
        if (enableEnergyDrop)
        {
            energy += energyChange * Time.deltaTime;
        }
        energy = Mathf.Clamp(energy, MinEnergy, 1f);
        energyBar.GetComponent<Image>().color = Color.Lerp(ZeroEnergyColor, FullEnergyColor, energy);
        energyBar.transform.localScale = Util.ChangeX(energyBar.transform.localScale, energy * energyBarOriginalXScale);

        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(ZeroEnergyColor, SchmoozeColor, energy);
        if (energy <= 0.5f && shakeCheck)
        {
            energyBar.gameObject.GetComponent<ShakeEffect>().Trigger(5f, 0.01f);
            shakeCheck = !shakeCheck;
            Debug.Log("will not shake again");
        }

        if (isSleeping && energy >= 1f)
        {
            ToggleSleeping();
        }
        else if (isSleeping && energy > 0.5f && !shakeCheck)
        {
            shakeCheck = !shakeCheck;
            Debug.Log("will shake again");
        }

        if (Tired && !createdIndicator || !Tired && createdIndicator)
        {
            IndicateBed();
        }

        if (!CanSchmooze())
        {
            GrowSchmoozeIcons();
        }
        else
        {
            FullSchmoozeIcons();
        }
    }

    public void ToggleSleeping()
    {
        isSleeping = !isSleeping;
        energyChange = isSleeping ? SleepEnergyChange : NormalEnergyChange;
        bedText.SetActive(isSleeping);
    }

    public bool Tired
    {
        get { return energy <= MinEnergy; }
    }

    public void LoseEnergyFromSchmooze()
    {
        energy -= SchmoozeEnergyDrop;
    }

    public bool CanSchmooze()
    {
        return SchmoozeHourCheck() && levelNum >= 3 && energy > MinEnergy;
    }

    private void IndicateBed()
    {
        if (isSleeping)
        {
            Destroy(indicator);
            createdIndicator = false;
        }
        else
        {
            indicator = CreateIndicator(bed, new(-1.15f, 0.5f, 0f));
            createdIndicator = true;
        }
    }

    private GameObject CreateIndicator(GameObject obj, Vector3 offset)
    {
        var indicator = Instantiate(cPrefab);
        indicator.GetComponent<FloatingAnim>().Init(obj, offset);
        return indicator;
    }

    public bool SchmoozeHourCheck()
    {
        int time = clock.clockTime.displayHour;
        return time != schmoozeHour;
    }

    public void SchmoozeHourOverwrite()
    {
        if (SchmoozeHourCheck())
        {
            schmoozeHour = clock.clockTime.displayHour;
        }
    }

    private void GrowSchmoozeIcons()
    {
        if (levelNum >= 3)
        {
            schmoozeCommand.color = NoSchmoozeColor;
            if (clock != null)
            {
                if (clock.clockTime.minute == 0)
                {
                    FullSchmoozeIcons();
                }
                else
                {
                    float change = (float)(clock.clockTime.minute) / 60;
                    schmoozeIndicator.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(IndicatorGreyedColor, Color.white, energy);
                }
            }
        }

    }

    private void FullSchmoozeIcons()
    {
        if (levelNum >= 3)
        {
            schmoozeCommand.color = SchmoozeColor;
            schmoozeIndicator.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}
