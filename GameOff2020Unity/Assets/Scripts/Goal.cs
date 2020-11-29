using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int styleReward = 50;
    [SerializeField] private Style style;
    [SerializeField] private UnityEvent onEnter = new UnityEvent();
    public bool exit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            onEnter.Invoke();
            gameObject.SetActive(false);
            //Play Random Cart Clear Voice Line
            int rand = Random.Range(0, 7);
            if (exit)
            {                style.AddStyle(styleReward);

                if (rand == 0)
                {
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear1", Random.Range(.95f, 1f));
                }
                else if (rand == 1)
                {
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear2", Random.Range(.95f, 1f));
                }
                else if (rand == 2)
                {
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear3", Random.Range(.95f, 1f));
                }
                else if (rand == 3)
                {
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear4", Random.Range(.95f, 1f));
                }
                else if (rand == 4)
                {
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear5", Random.Range(.95f, 1f));
                }
                else if (rand == 5)
                {
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear6", Random.Range(.95f, 1f));
                }
                else if (rand == 6)
                {


                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear7", Random.Range(.95f, 1f));
                }
                //Enable Lowpass
                FindObjectOfType<AudioManager>().lowPassEnable();
            }
            else
            {
                //Disable Lowpass
                FindObjectOfType<AudioManager>().lowPassDisable();
            }

        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
