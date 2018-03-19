using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControler : MonoBehaviour
{
    private Player player;
    public Text playerName;
    public Text Hp;
    public Text energy;
    public Text gold;

    // Use this for initialization
    private void Start()
    {
        player = Player.Instance;
        if (Monster.Instance != null)
        {
            Destroy(Monster.Instance.gameObject);
        }
        if (player != null)
        {
            playerName.text = "You play as: " + player.ShipName;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        Hp.text = "HP: " + player.HitPoint + "/" + player.MaxHitPoint;
        energy.text = "Ship energy: " + player.ShipEnergy.ToString();
        gold.text = "Gold:" + player.gold;
    }

    // Update is called once per frame
    private void Update()
    {
        RefreshUI();
    }
}