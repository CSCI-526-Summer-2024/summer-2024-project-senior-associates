using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public GameObject energyBar;
    public GameObject bed;
    public GameObject cPrefab;
    public bool enableEnergyDrop = true;
    private readonly float NormalEnergyChange = -0.02f;
    private readonly float SleepEnergyChange = 0.15f;
    private readonly Color ZeroEnergyColor = Color.red;
    private readonly Color FullEnergyColor = Color.green;
    private readonly float MinEnergy = 0.1f;
    private float energyChange;
    private float energy;
    private bool isSleeping = false; public bool IsSleeping => isSleeping;
    private float energyBarOriginalXScale;

    private GameObject indicator;

    void Start()
    {
        energy = 1f;
        energyChange = NormalEnergyChange;
        energyBarOriginalXScale = energyBar.transform.localScale.x;
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
        }
    }

    // public void StartSleeping()
    // {
    //     isSleeping = true;
    //     energyChange = SleepEnergyChange;
    // }

    // public void StopSleeping()
    // {
    //     isSleeping = false;
    //     energyChange = NormalEnergyChange;
    // }

    public void ToggleSleeping()
    {
        isSleeping = !isSleeping;
        energyChange = isSleeping ? SleepEnergyChange : NormalEnergyChange;
        IndicateBed();
    }

    public bool Tired
    {
        get { return energy <= MinEnergy; }
    }

    public void LoseEnergy()
    {
        energy -= 0.25f;
    }

    public bool CanSchmooze()
    {
        return energy >= 0.25f;
    }

    private void IndicateBed()
    {
        if (!isSleeping)
        {
            Destroy(indicator);
        }
        else
        {
            indicator = CreateIndicator(bed.gameObject, new(-1f, 0.5f, 0f));
        }
    }

    private GameObject CreateIndicator(GameObject obj, Vector3 offset, bool upSideDown = false)
    {
        var indicator = Instantiate(cPrefab);
        if (upSideDown)
        {
            indicator.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        indicator.GetComponent<FloatingAnim>().Init(obj, offset);
        return indicator;
    }
}
