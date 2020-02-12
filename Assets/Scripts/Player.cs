using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float timeInGame = 0;
    public float killedElite = 0;
    private Character character;
    private string shipName;
    private int hitPoint;
    private int shipEnergy;
    private int maxHitPoint;
    private int shieldMax = 0;
    private int currShield = 0;
    private SpriteRenderer sr;
    private bool isSpriteToRender = false;

    public int currShipEnergy;
    private int currBuff;
    public int gold = 800;
    public List<Item> inventory = new List<Item>();
    private JsonData itemBase;
    private static Player instance;

    public int ShieldMax { get { return shieldMax; } set { shieldMax = value; } }

    public int CurrShield
    {
        get { return currShield; }
        set
        {
            if (value > 0)
                currShield = value;
            else
                currShield = 0;

            if (shieldMax < CurrShield)
            {
                shieldMax = CurrShield;
            }
            if (currShield >= 60)
            {
                currShield = 60;
                shieldMax = 60;
            }
        }
    }

    public static Player Instance
    {
        get
        {
            return instance;
        }
    }

    public string ShipName
    {
        get
        {
            return shipName;
        }
    }

    public int HitPoint
    {
        get
        {
            return hitPoint;
        }
        set
        {
            if (value < maxHitPoint)
            {
                hitPoint = value;
            }
            else
            if (value >= maxHitPoint)
            {
                hitPoint = maxHitPoint;
            }
            else
            if (value <= 0)
            {
                hitPoint = 0;
            }
        }
    }

    public int MaxHitPoint
    {
        get
        {
            return maxHitPoint;
        }
        set
        {
            maxHitPoint += value;
            HitPoint += value;
        }
    }

    public int ShipEnergy
    {
        get
        {
            return shipEnergy;
        }
        set
        {
            shipEnergy = value;
        }
    }

    public SpriteRenderer Sr
    {
        get
        {
            return sr;
        }
    }

    public int CurrBuff
    {
        get
        {
            return currBuff;
        }

        set
        {
            currBuff = value;
            if (currBuff <= -3)
            {
                currBuff = -3;
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    private void Start()
    {
        itemBase = GameObject.FindObjectOfType<JsonData>();
        AddItemToInventort(itemBase.items[character.ItemID]);
    }

    public void AddItemToInventort(Item item)
    {
        inventory.Add(item);
        item.DoOnPickUp();
    }

    public void Initialize(Character xcharacter)
    {
        sr = GetComponent<SpriteRenderer>();
        character = xcharacter;
        shipName = character.name;
        maxHitPoint = character.startHitPoint;
        hitPoint = character.startHitPoint;
        shipEnergy = character.shipEnergy;
        sr.sprite = character.sprite;
        sr.enabled = isSpriteToRender;
        currShipEnergy = shipEnergy;
        sr.flipX = true;
    }

    // Update is called once per frame
    private void Update()
    {
        timeInGame += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Monster.Instance.CurrHp -= 10000;
        }
    }
}