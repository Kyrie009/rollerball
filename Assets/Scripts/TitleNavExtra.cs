using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleNavExtra : MonoBehaviour
{
    public GameObject levelSelectPanel;

    private void Start()
    {
        //set draw order
        LevelPanelOff();
    }

    public void GetLevelPanel()
    {
        levelSelectPanel.SetActive(true);
    }
    public void LevelPanelOff()
    {
        levelSelectPanel.SetActive(false);
    }
}
