using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControler : MonoBehaviour
{
    public void changeOnButtonPlay(int i)
    {
        Application.LoadLevel(i);
    }

    public void exitApplication()
    {
        Application.Quit();
    }
}