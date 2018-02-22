using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Character character;
    private string name;
    private int startHitPoint;
    private int maxHitPoint;
    private int shipEnergy;
    private SpriteRenderer sr;
    private bool isSpriteToRender = false;
    public static Player instance;

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
        name = character.name;
        startHitPoint = character.startHitPoint;
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