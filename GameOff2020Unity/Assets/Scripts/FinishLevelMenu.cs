using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject finishLevelMenuUI;

    public void Show()
    {
        finishLevelMenuUI.SetActive(true);
    }

    public void Hide()
    {
        finishLevelMenuUI.SetActive(false);
    }
}
