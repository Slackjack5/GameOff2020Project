using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private UnityEvent onExit = new UnityEvent();
    public bool exit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            onExit.Invoke();
            gameObject.SetActive(false);

            //Play Random Cart Clear Voice Line
            int rand = Random.Range(0, 7);
            if (exit)
            {
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
                    //Play Sound
                    FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-CartClear7", Random.Range(.95f, 1f));
                }
            }
            
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
