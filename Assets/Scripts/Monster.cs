using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public State move;
    private MapControler mapControler;

    [Tooltip("0-attack,1-shield,2-heal,3-buff,4-debuff")]
    public Sprite[] intenseSprite;//0-attack,1-shield,2-heal,3-buff,4-debuff

    [Tooltip("0-attack,1-shield,2-heal,3-buff,4-debuff")]
    public AudioClip[] stateClips;

    public AudioClip trump;

    private AudioClip clipToPlay;
    private static Monster instance = null;
    private Enemy enemy;
    public int MaxHp;
    private int currHp;
    private int currBuff = 0;
    private int currShield = 0;
    private int maxShield = 0;
    private float testingTimer = 0;
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
            if (currShield >= 60)
            {
                currShield = 60;
                maxShield = 60;
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

    public int CurrBuff
    {
        get { return currBuff; }
        set
        {
            currBuff = value;
            if (currBuff <= -15)
            {
                currBuff = -15;
            }
        }
    }

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
                int randAttac = Random.Range(enemy.dmgRange[0], enemy.dmgRange[1]);
                move = new AttackState(intenseSprite[0], randAttac);
                clipToPlay = stateClips[0];
                break;

            case 1:
                move = new ShieldState(intenseSprite[1], enemy.deff);
                clipToPlay = stateClips[1];
                break;

            case 2:
                move = new HealState(intenseSprite[2], enemy.healQuant);
                clipToPlay = stateClips[2];
                break;

            case 3:
                move = new BuffState(intenseSprite[3], enemy.buff);
                clipToPlay = stateClips[3];
                break;

            case 4:
                move = new DebuffState(intenseSprite[4], enemy.debuff);
                clipToPlay = stateClips[4];
                break;
        }
    }

    public void PlayTrump()
    {
        GetComponent<AudioSource>().clip = trump;
        GetComponent<AudioSource>().Play();
    }

    public void MakeAMove()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = clipToPlay;
        GetComponent<AudioSource>().Play();
        move.DoAction();
        RandAState();
    }

    public void Initialize(Enemy xenemy)
    {
        enemy = xenemy;
        MaxHp = enemy.startHP;
        currHp = MaxHp;
        RandAState();
    }

    // Use this for initialization
    private void Start()
    {
        mapControler = GameObject.Find("MapControler").GetComponent<MapControler>();
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

    // Update is called once per frame
    private void Update()
    {
    }

    public void Kill()
    {
        mapControler.pointToGo.GetInstantion().GetComponent<PointControler>().pst.DoOnEnd();
        Destroy(gameObject);
    }
}