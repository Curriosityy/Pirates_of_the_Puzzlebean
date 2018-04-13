using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPauseControler : MonoBehaviour
{
    public enum MapState
    {
        map, pause
    }

    public GameObject pauseScreen;
    public MapState mapState;

    private void Start()
    {
        mapState = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mapState == MapState.map)
            {
                mapState = MapState.pause;
                pauseScreen.SetActive(true);
            }
            else
            {
                mapState = MapState.map;
                pauseScreen.SetActive(false);
            }
        }
    }

    public void OnClickSwapScenes(int i)
    {
        MenuControler.ChangeBetweenScenes(i);
    }

    public void OnStartClick()
    {
        mapState = MapState.map;
        pauseScreen.SetActive(false);
    }
}