using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public Slider hpSliderArea;
    public Text hpTextArea;
    public Image playerImageArea;
    private Player player;
    public Text energyText;
    public Slider shieldSlider;
    public Text shieldText;
    public Text buffText;
    public Image buffImage;
    public Text goldText;

    // Use this for initialization
    private void Start()
    {
        player = Player.Instance;
        if (player != null)
        {
            RefreshAll();
            playerImageArea.sprite = player.Sr.sprite;
            playerImageArea.preserveAspect = true;
            playerImageArea.transform.localRotation = Quaternion.Euler(0, 180, 0);
            goldText.text = player.gold.ToString();
        }
    }

    private void RefreshAll()
    {
        hpSliderArea.maxValue = player.MaxHitPoint;
        hpSliderArea.value = player.HitPoint;
        hpTextArea.text = player.HitPoint + "/" + player.MaxHitPoint;
        energyText.text = player.currShipEnergy.ToString();
        shieldSlider.maxValue = player.ShieldMax;
        shieldSlider.value = player.CurrShield;
        buffText.text = player.CurrBuff.ToString();
        shieldSlider.maxValue = player.ShieldMax;
        shieldSlider.value = player.CurrShield;
        goldText.text = player.gold.ToString() + "$";
        shieldText.text = player.CurrShield + "/" + player.ShieldMax;
        if (player.CurrBuff == 0)
        {
            buffImage.gameObject.SetActive(false);
        }
        else
        {
            buffImage.gameObject.SetActive(true);
        }
        if (player.CurrBuff > 0)
        {
            buffText.color = Color.green;
        }
        else
        {
            buffText.color = Color.red;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            /* if (player.currShipEnergy != int.Parse(energyText.text))
             {
                 RefreshAll();
             }*/
            RefreshAll();
        }
    }
}