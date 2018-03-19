using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControler : MonoBehaviour
{
    public static bool pause = false;

    public GameObject pauseMenuUI;
    public GameObject loadMap;
    public GameObject winScreen;
    public GameObject loseScreen;
    private float ticker = 0;

    private void Start()
    {
        loadMap.SetActive(true);
        pause = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (BattleControler.battleState != BattleState.creatingMap && ticker >= 2f)
        {
            loadMap.SetActive(false);
        }
        else
        {
            ticker += Time.deltaTime;
            Debug.Log(ticker);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BattleControler.battleState == BattleState.battle)
            {
                Debug.Log(BattleControler.battleState);
                setPause(0);
                BattleControler.battleState = BattleState.pause;
            }
            else
            if (BattleControler.battleState == BattleState.pause)
            {
                Debug.Log(BattleControler.battleState);
                setPause(1);
                BattleControler.battleState = BattleState.battle;
            }
        }
        if (BattleControler.battleState == BattleState.win)
        {
            winScreen.SetActive(true);
            if (Input.anyKey)
            {
                MapControler mapControler = GameObject.FindObjectOfType<MapControler>();
                mapControler.pointOfStaying = mapControler.pointToGo;
                mapControler.pointOfStaying.GetInstantion().GetComponent<PointControler>().isVisited = true;

                OnClickSwapScenes(1);
            }
        }
        if (BattleControler.battleState == BattleState.lose)
        {
            loseScreen.SetActive(true);
            if (Input.anyKey)
            {
                OnClickSwapScenes(0);
            }
        }
    }

    public void setPause(int i)
    {
        pause = !pause;
        pauseMenuUI.SetActive(pause);
        Time.timeScale = i;
    }

    public void OnClickSwapScenes(int i)
    {
        Time.timeScale = 1;
        MenuControler.ChangeBetweenScenes(i);
    }
}