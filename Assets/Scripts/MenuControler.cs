using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour
{
    public GameObject shipSelect;
    public GameObject mainMenu;
    public GameObject player;

    public Character[] character;

    public static void ChangeBetweenScenes(int i)
    {
        PauseControler.pause = false;
        BattleControler.coIsRunning = false;
        GemControler.anyCoIsRun.Clear();
        //Application.LoadLevel(i);
        SceneManager.LoadScene(i);
    }

    public void OnSelectedCharacter(int i)
    {
        GameObject instatntiatedPlayer = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        instatntiatedPlayer.GetComponent<Player>().Initialize(character[i]);
        ChangeBetweenScenes(1);
    }

    public void ChangeBetweenShipSelectAndMenuPanel(bool shipOrMenu)//true menu, false ship
    {
        mainMenu.SetActive(shipOrMenu);
        shipSelect.SetActive(!shipOrMenu);
    }

    public void exitApplication()
    {
        Application.Quit();
    }
}