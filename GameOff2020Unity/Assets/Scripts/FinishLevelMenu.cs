using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject finishLevelMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        finishLevelMenuUI.SetActive(true);
    }
}
