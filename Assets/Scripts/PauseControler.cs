using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControler : MonoBehaviour
{
    public static bool pause = false;

    public GameObject pauseMenuUI;

    private void Start()
    {
        pause = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
            {
                setPause(0);
            }
            else
            {
                setPause(1);
            }
        }
    }

    public void setPause(int i)
    {
        pause = !pause;
        pauseMenuUI.SetActive(pause);
        Time.timeScale = 1;
    }
}