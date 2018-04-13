using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseControler : MonoBehaviour
{
    public static bool pause = false;
    private static bool itemOnce = true;
    public GameObject pauseMenuUI;
    public GameObject loadMap;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject lootSprite;
    private float ticker = 0;
    public Text lootText;

    private void Start()
    {
        itemOnce = true;
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
            if (itemOnce)
            {
                Player.Instance.inventory.ForEach(item => { item.DoOnBattleEnd(); });
                lootText.text = BattleControler.goldLoot.ToString();
                if (BattleControler.lootItem != null)
                {
                    lootSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>(BattleControler.lootItem.spritePath);
                    lootSprite.SetActive(true);
                }
                else
                {
                    lootSprite.SetActive(false);
                }
                itemOnce = false;
            }

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