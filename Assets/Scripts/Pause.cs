using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    bool isPaused = false;

    // Start is called before the first frame update
    private void Start()
    {
        pausePanel.SetActive(false);
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0; // sets all time based things to stop, animation,time,physics...
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1; //set everthing to normal time again
        }
    }
}
