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

    // Use this for initialization
    private void Start()
    {
        player = Player.Instance;
        if (player != null)
        {
            Debug.Log(player);
            playerName.text = "You play as: " + player.ShipName;
            Hp.text = "HP: " + player.HitPoint + "/" + player.MaxHitPoint;
            energy.text = "Ship energy: " + player.ShipEnergy.ToString();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}