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
    public TextMeshProUGUI textMesh;
    public int tier = 1;

    //Private
    public int timerinitiator = 0;
    private int decayNumber = 0;
    private int decayNumber2 = 0;
    private bool timerActive = false;
    private bool tickAlternate = false;

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

    public void TimerRestart()
    {
        decayNumber = 0;
        decayNumber2 = 0;
        timerinitiator = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set our maximum Value Slider then use Style Total as our current amount of style
        styleTotal = 0;
    }

    private void FixedUpdate()
    {
        //If we have style and the player hasn't killed anything in a while, start a countdown
        if (styleTotal > 0)
        {
            SetStyle(styleTotal);

            if (timerinitiator == styleDecay * 5)
            {
                TimerRestart();
                timerActive = true;
            }

            if (timerActive == true)
            {
                //Count
                if (decayNumber == styleDecay)
                {
                    decayNumber = 0;
                    decayNumber2 += 1;

                    //Play Sound
                    if (tickAlternate == false)
                    {
                        FindObjectOfType<AudioManager>().PlaySound("TimerTick", Random.Range(.95f, 1f));
                        tickAlternate = true;
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().PlaySound("TimerTock", Random.Range(.95f, 1f));
                        tickAlternate = false;
                    }
                }

                //Double Time Tick
                if ((decayNumber == styleDecay / 2) && decayNumber2 >= styleDecay / 2)
                {
                    //Play Sound
                    if (tickAlternate == false)
                    {
                        FindObjectOfType<AudioManager>().PlaySound("TimerTick", Random.Range(.95f, 1f));
                        tickAlternate = true;
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().PlaySound("TimerTock", Random.Range(.95f, 1f));
                        tickAlternate = false;
                    }
                }

                //If decay 2 reaches style decay then reset style to rank D
                if (decayNumber2 == styleDecay)
                {
                    ResetStyle();
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("TimerLost", 1f);
                    //Play Random Quip
                    int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            //Play Sound
                            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankDown1", Random.Range(.95f, 1f));
                        }
                        else if (rand == 1)
                        {
                            //Play Sound
                            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankDown2", Random.Range(.95f, 1f));
                        }
                        else if (rand == 2)
                        {
                        //Play Sound
                            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankDown3", Random.Range(.95f, 1f));
                        }
                    
                }
            }

            //If timer hasn't started, count towards starting it
            if (timerActive)
            {
                decayNumber++;
            }
            else
            {
                timerinitiator++;
            }
        }
    }

    private void Update()
    {
        //If we reach 100 Style, take us to the next tier
        if (styleTotal >= 100 && tier < 5)
        {
            tier += 1;
            styleTotal = 10;

            if (tier == 2)
            {
                FindObjectOfType<AudioManager>().PlaySound("TimerRankUp", 1f);
            }
            else if (tier == 3)
            {
                FindObjectOfType<AudioManager>().PlaySound("TimerRankUp", 1.05f);
            }
            else if (tier == 4)
            {
                FindObjectOfType<AudioManager>().PlaySound("TimerRankUp", 1.1f);
            }
            else if (tier == 5)
            {
                FindObjectOfType<AudioManager>().PlaySound("TimerRankUp", 1.15f);
            }

            //Play Random Quip
            int rand = Random.Range(0, 6);
            if (rand == 0)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankUp1", Random.Range(.95f, 1f));
            }
            else if (rand == 1)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankUp2", Random.Range(.95f, 1f));
            }
            else if (rand == 2)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankUp3", Random.Range(.95f, 1f));
            }
            else if (rand == 3)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankUp4", Random.Range(.95f, 1f));
            }
            else if (rand == 4)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankUp5", Random.Range(.95f, 1f));
            }
            else if (rand == 5)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-RankUp6", Random.Range(.95f, 1f));
            }
        }
        else if (styleTotal <= 0 && tier > 1)
        {
            tier = 1;
        }
        else if (styleTotal > 100 && tier == 5)
        {
            styleTotal = 100;
        }

        //Set Text for Rank
        if (tier == 1)
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


        if (GameManager.playerIsDead)
        {
            ResetStyle();
        }
    }

    private void ResetStyle()
    {
        styleTotal = 0;
        decayNumber = 0;
        decayNumber2 = 0;
        timerinitiator = 0;

        SetStyle(styleTotal);
    }

    public void AddStyle(int style)
    {
        if (tier <= 5)
        {
            styleTotal += style;
            TimerRestart();
        }
    }
}
