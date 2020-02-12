using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRefresher : MonoBehaviour
{
    public Text text;
    private Player player;

    // Use this for initialization
    private void Start()
    {
        player = Player.Instance;
    }

    // Update is called once per frame
    private void Update()
    {
        int min = (int)(player.timeInGame / 60);
        int sec = (int)(player.timeInGame % 60);
        if (min < 10)
        {
            text.text = "0" + min.ToString();
        }
        else
        {
            text.text = min.ToString();
        }
        if (sec < 10)
        {
            text.text += ":0" + sec.ToString();
        }
        else
        {
            text.text += ":" + sec.ToString();
        }
    }
}