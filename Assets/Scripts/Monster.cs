using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public State move;

    [Tooltip("0-attack,1-shield,2-heal,3-buff,4-debuff")]
    public Sprite[] intenseSprite;//0-attack,1-shield,2-heal,3-buff,4-debuff

    private static Monster instance;
    private Enemy enemy;
    public int MaxHp;
    private int currHp;
    private int currBuff = 0;
    private int currShield = 0;
    private int maxShield = 0;

    private void RandAState()
    {
        float rand = Random.Range(0f, 1f);
        float sum = 0;
        int moveNumber = 0;
        for (int i = 0; i < enemy.moveProb.Length; i++)
        {
            sum += enemy.moveProb[i];
            if (rand <= sum)
            {
                moveNumber = i;
                break;
            }
        }
        switch (moveNumber)
        {
            case 0:
                move = new AttackState(intenseSprite[0], Random.Range(enemy.dmgRange[0], enemy.dmgRange[1]));
                break;

            case 1:
                move = new ShieldState(intenseSprite[1], enemy.deff);
                break;

            case 2:
                move = new HealState(intenseSprite[2], enemy.healQuant);
                break;

            case 3:
                move = new BuffState(intenseSprite[3], enemy.buff);
                break;

            case 4:
                move = new DebuffState(intenseSprite[4], enemy.debuff);
                break;
        }
    }

    public void MakeAMove()
    {
        move.DoAction();
        RandAState();
    }

    public int MaxShield { get { return maxShield; } set { maxShield = value; } }

    public int CurrShield
    {
        get
        {
            return currShield;
        }
        set
        {
            if (value > 0)
                currShield = value;
            else
                currShield = 0;

            if (maxShield < CurrShield)
            {
                maxShield = CurrShield;
            }
        }
    }

    public int CurrHp
    {
        get { return currHp; }
        set
        {
            if (value < enemy.startHP)
            {
                currHp = value;
            }
            if (value > enemy.startHP)
            {
                currHp = enemy.startHP;
            }
            if (value <= 0)
            {
                currHp = 0;
            }
        }
    }

    public int CurrBuff { get { return currBuff; } set { currBuff = value; } }

    public Enemy Enemy
    {
        get
        {
            return enemy;
        }
    }

    public static Monster Instance
    {
        get
        {
            return instance;
        }
    }

    public void Initialize(Enemy xenemy)
    {
        enemy = xenemy;
        MaxHp = enemy.startHP;
        currHp = MaxHp;
    }

    // Use this for initialization
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            RandAState();
        }
        else
        {
            Destroy(gameObject.transform);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}