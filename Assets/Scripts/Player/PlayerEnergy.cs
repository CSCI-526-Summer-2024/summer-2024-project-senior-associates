using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public GameObject energyBar;
    public GameObject bed;
    public GameObject cPrefab;
    public GameObject bedText;
    public GameObject player;
    public bool enableEnergyDrop = true;
    private readonly float NormalEnergyChange = -0.02f;
    private readonly float SleepEnergyChange = 0.15f;
    private readonly float SchmoozeEnergyDrop = 0.3f;
    private readonly Color ZeroEnergyColor = Color.red;
    private readonly Color FullEnergyColor = Color.green;
    private readonly float MinEnergy = 0.1f;
    private float energyChange;
    private float energy;
    private bool isSleeping = false; public bool IsSleeping => isSleeping;
    private float energyBarOriginalXScale;
    private GameObject indicator;
    private int levelNum;
    private bool createdIndicator = false;

    void Start()
    {
        energy = 1f;
        energyChange = NormalEnergyChange;
        energyBarOriginalXScale = energyBar.transform.localScale.x;
        levelNum = Util.GetCurrentLevelNum();
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
            energy = Mathf.Clamp(energy, MinEnergy, 1f);
            energyBar.GetComponent<Image>().color = Color.Lerp(ZeroEnergyColor, FullEnergyColor, energy);
            energyBar.transform.localScale = Util.ChangeX(energyBar.transform.localScale, energy * energyBarOriginalXScale);

            if (isSleeping && energy >= 1f)
            {
                ToggleSleeping();
            }

            if (Tired && !createdIndicator || !Tired && createdIndicator)
            {
                IndicateBed();
            }
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
        return levelNum >= 3 && energy > MinEnergy;
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
}
