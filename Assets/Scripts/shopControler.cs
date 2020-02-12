using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class shopControler : MonoBehaviour
{
    public Button[] buttonItems;
    private Player player;
    private JsonData items;
    private List<Item> randomedItems = new List<Item>();
    private List<int> cost = new List<int>();
    private ItemControler ic;

    // Use this for initialization
    private void Start()
    {
        player = Player.Instance;
        items = GameObject.FindObjectOfType<JsonData>();
        ic = GameObject.FindObjectOfType<ItemControler>();
        RandomItemToBuy();
    }

    public void RandomItemToBuy()
    {
        randomedItems.Clear();
        cost.Clear();
        List<Item> itemToRandom = new List<Item>();
        items.itemsValue.ForEach(item =>
        {
            if (!player.inventory.Contains(item) && item.id != 3)
            {
                itemToRandom.Add(item);
            }
        });
        if (itemToRandom.Count <= 3)
        {
            buttonItems[5].interactable = false;
        }
        for (int i = 0; i < 3; i++)
        {
            if (itemToRandom.Count > 0)
            {
                int rand = Random.Range(0, itemToRandom.Count);
                randomedItems.Add(itemToRandom[rand]);
                itemToRandom.Remove(randomedItems[i]);
                buttonItems[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(randomedItems[i].spritePath);
                ItemInfoBox iib = buttonItems[i].GetComponent<ItemInfoBox>();
                iib.itemName = randomedItems[i].name;
                iib.text = randomedItems[i].description;
                cost.Add(randomedItems[i].rarity.GetHashCode() * 100 + 100 + Random.Range(100, 200));
                buttonItems[i].GetComponentInChildren<Text>().text = cost[i] + " gold";
                buttonItems[i].interactable = true;
                buttonItems[i].transform.Find("Sold").gameObject.SetActive(false);
                iib.rare = randomedItems[i].rarity;
            }
            else
            {
                buttonItems[i].gameObject.SetActive(false);
            }
        }
    }

    public void RandomOnClick(int x)
    {
        if (player.gold >= x)
        {
            RandomItemToBuy();
            player.gold -= x;
            buttonItems[5].interactable = false;
            buttonItems[5].transform.Find("Sold").gameObject.SetActive(true);
        }
    }

    public void AddMaxHpOnClick(int x)
    {
        if (player.gold >= x)
        {
            player.MaxHitPoint = 10;
            player.gold -= x;
            buttonItems[4].interactable = false;
            buttonItems[4].transform.Find("Sold").gameObject.SetActive(true);
        }
    }

    public void HealOnClick(int x)
    {
        if (player.gold >= x)
        {
            player.gold -= x;
            int maxHp = player.MaxHitPoint;
            maxHp = (maxHp * 30) / 100;
            player.HitPoint += maxHp;
            buttonItems[3].interactable = false;
            buttonItems[3].transform.Find("Sold").gameObject.SetActive(true);
        }
    }

    public void Item1OnClick()
    {
        if (player.gold >= cost[0])
        {
            player.AddItemToInventort(randomedItems[0]);
            player.gold -= cost[0];
            buttonItems[0].interactable = false;
            ic.AddItemToBar(randomedItems[0]);
            buttonItems[0].transform.Find("Sold").gameObject.SetActive(true);
        }
    }

    public void Item2OnClick()
    {
        if (player.gold >= cost[1])
        {
            player.AddItemToInventort(randomedItems[1]);
            player.gold -= cost[1];
            buttonItems[1].interactable = false;
            ic.AddItemToBar(randomedItems[1]);
            buttonItems[1].transform.Find("Sold").gameObject.SetActive(true);
        }
    }

    public void Item3OnClick()
    {
        if (player.gold >= cost[2])
        {
            player.AddItemToInventort(randomedItems[2]);
            player.gold -= cost[2];
            buttonItems[2].interactable = false;
            ic.AddItemToBar(randomedItems[2]);
            buttonItems[2].transform.Find("Sold").gameObject.SetActive(true);
        }
    }

    public void BackOnClick()
    {
        MenuControler.ChangeBetweenScenes(1);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}