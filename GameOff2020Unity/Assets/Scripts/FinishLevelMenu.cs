using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject finishLevelMenuUI;

    public void Show()
    {
        finishLevelMenuUI.SetActive(true);
        //Audio for Completion
        /*
        int rand = Random.Range(0, 5);
        if (rand == 0)
        {
            //Play Sound
            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-Completion1", Random.Range(.95f, 1f));
        }
        else if (rand == 1)
        {
            //Play Sound
            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-Completion2", Random.Range(.95f, 1f));
        }
        else if (rand == 2)
        {
            //Play Sound
            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-Completion3", Random.Range(.95f, 1f));
        }
        else if (rand == 3)
        {
            //Play Sound
            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-Completion4", Random.Range(.95f, 1f));
        }
        else if (rand == 4)
        {
            //Play Sound
            FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-Completion5", Random.Range(.95f, 1f));
        }
        */
    }

    public void Hide()
    {
        finishLevelMenuUI.SetActive(false);
    }
}
