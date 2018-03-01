using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGuiManager : MonoBehaviour
{
    private Monster monster;
    public Slider hpSliderArea;
    public Text hpTextArea;
    public Image enemyImageArea;
    public Slider shieldSlider;
    public Text shieldText;
    public Text buffText;
    public Image intenceImage;
    public Text intenceQuantity;
    public Image buffImage;

    // Use this for initialization
    private void Start()
    {
        monster = Monster.Instance;
        if (monster != null)
        {
            enemyImageArea.sprite = monster.Enemy.enemyLook;
            enemyImageArea.preserveAspect = true;
            Refresh();
        }
    }

    private void Refresh()
    {
        hpSliderArea.maxValue = monster.MaxHp;
        hpSliderArea.value = monster.CurrHp;
        hpTextArea.text = monster.CurrHp + "/" + monster.MaxHp;
        shieldSlider.maxValue = monster.MaxShield;
        shieldSlider.value = monster.CurrShield;
        shieldText.text = monster.CurrShield + "/" + monster.MaxShield;

        buffText.text = monster.CurrBuff.ToString();
        intenceImage.sprite = monster.move.spriteOfMove;
        intenceQuantity.text = monster.move.moveQuantity.ToString();
        if (monster.CurrBuff == 0)
        {
            buffImage.gameObject.SetActive(false);
        }
        else
        {
            buffImage.gameObject.SetActive(true);
        }
        if (monster.CurrBuff > 0)
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
        if (intenceImage.sprite != monster.move.spriteOfMove)
        {
            Refresh();
        }
        else
        if (monster.CurrHp != hpSliderArea.value)
        {
            Refresh();
        }
        else if (monster.CurrShield != shieldSlider.value)
        {
            Refresh();
        }
    }
}