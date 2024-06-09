using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private readonly Vector3 EnergyBarOffset = new(1.25f, 0.6f, 0);
    private float energyDrop;

    public float energy = 1f;
    private float energyMax = 1f;
    private SpriteRenderer spriteRenderer;

    private Color tired;

    private Color full;

    private GameObject player;
    public GameObject energyBar;
    // Start is called before the first frame update
    void Start()
    {
        //find player 
        player = GameObject.FindWithTag("Player");

        //set energy drop
        energyDrop = 0.05f * Time.deltaTime;

        //create color gradient between blue and grey

        tired = new Color(116f / 255f, 116f / 255f, 116f / 255f);
        full = new Color(105f / 255f, 222f / 255f, 255f / 255f);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        energyBar.transform.localPosition = player.transform.localPosition + EnergyBarOffset;
        EnergyDecrease();
    }

    //Energy bar decreases in size and should be turning greyish
    void EnergyDecrease()
    {
        if (energy > 0f)
        {
            Vector3 scale = energyBar.transform.localScale;
            scale.x = energy / energyMax;
            energyBar.transform.localScale = scale;

            energy -= energyDrop;

            if (energy > 0f)
            {
                energyBar.GetComponent<Image>().color = Color.Lerp(tired, full, energy);
                spriteRenderer.color = Color.Lerp(tired, full, energy);
            }
        }
        else
        {
            energy = 0f;
            PlayerControl playControl = player.GetComponent<PlayerControl>();
            playControl.tired = true;
        }
    }

    //call this to replenish energy
    //should be called when colliding with Bed
    public void Sleep()
    {
        energy = 1f;
        PlayerControl playControl = player.GetComponent<PlayerControl>();
        playControl.tired = false;
    }
}
