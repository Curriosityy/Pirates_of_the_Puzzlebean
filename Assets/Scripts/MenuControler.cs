using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour
{
    public void changeOnButtonPlay(int i)
    {
        PauseControler.pause = false;
        BattleControler.coIsRunning = false;
        GemControler.anyCoIsRun.Clear();
        //Application.LoadLevel(i);
        SceneManager.LoadScene(i);
    }

    public void exitApplication()
    {
        Application.Quit();
    }
}