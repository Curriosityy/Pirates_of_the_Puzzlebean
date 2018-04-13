﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Character character;
    private string shipName;
    private int hitPoint;
    private int shipEnergy;
    private int maxHitPoint;
    private int shieldMax = 0;
    private int currShield = 0;
    private SpriteRenderer sr;
    private bool isSpriteToRender = false;
    private static Player instance;
    public int currShipEnergy;
    public int currBuff;
    public int gold = 150;
    public List<Item> inventory = new List<Item>();

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
            hitPoint += value;
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
            shipEnergy += value;
        }
    }

    public SpriteRenderer Sr
    {
        get
        {
            return sr;
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
    }
}