using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Style : MonoBehaviour
{
    //Public
    public int styleTotal = 0;
    public Slider slider;
    public int styleDecay = 10;

    //Private
    private int decayNumber = 0;
    private int tier = 1;
    public TextMeshProUGUI textMesh;

    //Sets the Maximum Value of the Slider
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    //Sets the Current Value of our Slider
    public void SetStyle(int style)
    {
        slider.value = style;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Set our maximum Value Slider then use Style Total as our current amount of style
        styleTotal = 100;
        SetMaxHealth(styleTotal);
        styleTotal = 10;
    }
    private void FixedUpdate()
    {
        //If we style, slowly drain it
        if (styleTotal > 0)
        {
            if (decayNumber == styleDecay)
            {
                decayNumber = 0;
                SetStyle(styleTotal);
                styleTotal -= 1;
            }
            decayNumber++;
            Debug.Log(tier);
        }
    }
    private void Update()
    {
        //If we reach 100 Style, take us to the next tier
        if (styleTotal >= 100 && tier<5)
        {
            tier += 1;
            styleTotal = 10;
        }
        else if (styleTotal<=0 && tier>1)
        {
            tier = 1;
        }
        else if (styleTotal > 100 && tier==5)
        {
            styleTotal = 100;
        }

        if (Input.GetKeyDown("space") && tier<=5)
        {
            styleTotal += 10;
        }
        //Set Text for Rank
        if (tier==1)
        {
            textMesh.text = "D-Rank";
        }
        else if (tier == 2)
        {
            textMesh.text = "C-Rank";
        }
        else if (tier == 3)
        {
            textMesh.text = "B-Rank";
        }
        else if (tier == 4)
        {
            textMesh.text = "A-Rank";
        }
        else if (tier == 5)
        {
            textMesh.text = "S-Rank";
        }

    }
}
