using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreCount : MonoBehaviour
{
    public Text textt;
    private float timer;

    // Use this for initialization
    private void Start()
    {
        Player player = Player.Instance;
        float score = 3000 / player.timeInGame * (player.gold * 10 + player.inventory.Count * 200 + player.killedElite * 1000);
        textt.text += ((int)score).ToString();
        timer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKey && timer >= 3f)
        {
            SceneManager.LoadScene(0);
        }
    }
}