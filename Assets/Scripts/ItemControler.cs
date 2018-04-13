using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControler : MonoBehaviour
{
    public GameObject itemGameObject;
    private Dictionary<Item, Text> listOfTick = new Dictionary<Item, Text>();
    private Player player;
    private int i = 0;
    private int j = 0;

    // Use this for initialization
    private void Start()
    {
        player = Player.Instance;
        foreach (Item item in player.inventory)
        {
            AddItemToBar(item);
        }
    }

    public void AddItemToBar(Item item)
    {
        GameObject instance = Instantiate(itemGameObject, Vector3.zero, Quaternion.identity, transform) as GameObject;
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        Vector2 pos = rectTransform.localPosition;
        pos.x = i * rectTransform.rect.width + 50;
        pos.y = j * rectTransform.rect.height - 50;

        if (((pos.x + 50) * gameObject.GetComponent<RectTransform>().localScale.x) >= gameObject.GetComponent<RectTransform>().rect.width)
        {
            j -= 1;
            i = 0;
            pos.x = i * rectTransform.rect.width + 50;
            pos.y = j * rectTransform.rect.height - 50;
        }
        rectTransform.localPosition = pos;
        int tick = item.tick;
        if (tick >= 0)
        {
            Text text2 = instance.GetComponentInChildren<Text>(true);
            text2.text = tick.ToString();
            text2.gameObject.SetActive(true);
            listOfTick.Add(item, text2);
        }
        instance.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        ItemInfoBox infoBox = instance.GetComponent<ItemInfoBox>();
        infoBox.itemName = item.name;
        infoBox.rare = item.rarity;
        infoBox.text = item.description;
        i += 1;
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var tick in listOfTick)
        {
            tick.Value.text = tick.Key.tick.ToString();
        }
    }
}