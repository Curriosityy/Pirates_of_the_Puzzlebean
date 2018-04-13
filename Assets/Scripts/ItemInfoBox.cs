using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfoBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    public string text;
    public Rarity rare;
    private float timer = 0;
    private bool entered = false;
    private bool show = false;
    public Text nameText;
    public Text destText;
    public GameObject infoPanel;
    public Color[] color;

    // Use this for initialization
    private void Start()
    {
        nameText.text = itemName;
        destText.text = text;
        nameText.color = color[rare.GetHashCode()];
    }

    // Update is called once per frame
    private void Update()
    {
        nameText.text = itemName;
        destText.text = text;
        nameText.color = color[rare.GetHashCode()];
        if (entered)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                infoPanel.SetActive(true);
            }
            else
            {
                infoPanel.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        entered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        timer = 0;
        entered = false;
        infoPanel.SetActive(false);
    }
}