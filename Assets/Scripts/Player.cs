using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Character character;
    private string shipName;
    private int hitPoint;
    private int shipEnergy;
    private int maxHitPoint;
    private SpriteRenderer sr;
    private bool isSpriteToRender = false;
    private static Player instance;

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
    }

    public int MaxHitPoint
    {
        get
        {
            return maxHitPoint;
        }
    }

    public int ShipEnergy
    {
        get
        {
            return shipEnergy;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Character xcharacter)
    {
        sr = GetComponent<SpriteRenderer>();
        character = xcharacter;
        shipName = character.name;
        hitPoint = character.startHitPoint;
        maxHitPoint = character.startHitPoint;
        shipEnergy = character.shipEnergy;
        sr.sprite = character.sprite;
        sr.enabled = isSpriteToRender;
        sr.flipX = true;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}