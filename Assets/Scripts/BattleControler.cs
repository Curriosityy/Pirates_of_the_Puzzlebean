using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class BattleControler : MonoBehaviour
{
    public int superNovaCounter;

    [Tooltip("0-attack,1-shield,2-heal,3-buff,4-debuff,5-gold,6-nova")]
    public AudioClip[] audioClips;

    private AudioSource[] audioSource;
    public GameObject superNovaAnim;
    public GameObject novaPosition;
    public static BattleState battleState;
    private MapControler mapControler;
    public GameObject[] gems;
    private int[] destroyedGemCount;//0-attack 1-heal 2-shield 3-buff 4-gold 5-debuff
    public int row;
    public int col;
    private GemControler[,] board;
    public static bool finded = false;
    private Transform boardHolder;
    public float[] probabilityOfEachGem;
    public static bool coIsRunning = false;
    public static bool isMapFull = false;
    private bool cleared = false;
    private Player player;
    private Monster monster;
    private Transform playerImageTransform;
    private Transform enemyImageTransform;
    public static int goldLoot = 0;
    public static Item lootItem = null;
    private bool monsterSong = false;
    private bool superNova = false;

    private void SetNeightbours()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (i != 0)
                {
                    board[i, j].neighbors[0] = board[i - 1, j].gameObject;
                }
                if (j != 0)
                {
                    board[i, j].neighbors[3] = board[i, j - 1].gameObject;
                }
                if (i != col - 1)
                {
                    board[i, j].neighbors[2] = board[i + 1, j].gameObject;
                }
                if (j != row - 1)
                {
                    board[i, j].neighbors[1] = board[i, j + 1].gameObject;
                }
            }
        }
    }

    private void Start()
    {
        //player.currShipEnergy = player.ShipEnergy;
        lootItem = null;
        goldLoot = 0;
        battleState = BattleState.creatingMap;
        coIsRunning = false;
        isMapFull = false;
        cleared = false;
        finded = false;
        board = new GemControler[col, row + 1];
        GenerateBoard();
        player = Player.Instance;
        monster = Monster.Instance;
        destroyedGemCount = new int[gems.Length];
        player.CurrBuff = 0;
        player.CurrShield = 0;
        player.ShieldMax = 0;
        PopoutCreator.Initialize();
        mapControler = GameObject.Find("MapControler").GetComponent<MapControler>();
        playerImageTransform = GameObject.Find("PlayerImage").transform;
        enemyImageTransform = GameObject.Find("EnemyImage").transform;
        player.currShipEnergy = player.ShipEnergy;
        player.inventory.ForEach(item => { item.DoOnBattleStart(); });
        audioSource = GetComponents<AudioSource>();
    }

    private void GenerateBoard()
    {
        boardHolder = new GameObject("board").transform;
        boardHolder.transform.position = new Vector2(-1.85f, 6.47f);
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GenerateGem(i, j, 0);
            }
        }
        SetNeightbours();
    }

    private int randomizeGem()
    {
        float sum = 0;
        float random = (float)Random.Range(0f, 1f);
        for (int i = 0; i < gems.Length; i++)
        {
            sum += probabilityOfEachGem[i];
            if (sum >= random)
            {
                return i;
            }
        }
        return gems.Length - 1;
    }

    private void GenerateGem(int i, int j, int shift)
    {
        float x, y;
        GetXAndYOnBoard(i, j + shift, out x, out y);
        board[i, j] = Instantiate(gems[randomizeGem()], new Vector2(x, y), Quaternion.identity).GetComponent<GemControler>();
        board[i, j].transform.SetParent(boardHolder);
    }

    private void CheckForMatch()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] != null)
                {
                    CheckForMatch(board[i, j]);
                }
            }
        }
    }

    private void CheckForMatch(GemControler xgemControler)
    {
        xgemControler.SearchForMatch();
    }

    private void DestroyMatches()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] != null)
                {
                    if (board[i, j].matched && !board[i, j].move)
                    {
                        List<GameObject> grouped = board[i, j].GroupMatched();

                        //board[i, j].gameObject.SetActive(false);
                        if (battleState == BattleState.battle)
                        {
                            switch (board[i, j].gameObject.tag)
                            {
                                case "gem6":
                                    destroyedGemCount[0] += grouped.Count;
                                    break;

                                case "gem3":
                                    destroyedGemCount[1] += grouped.Count;
                                    break;

                                case "gem4":
                                    destroyedGemCount[2] += grouped.Count;
                                    break;

                                case "gem1":
                                    destroyedGemCount[3] += grouped.Count;
                                    break;

                                case "gem2":
                                    destroyedGemCount[4] += grouped.Count;
                                    break;

                                case "gem5":
                                    destroyedGemCount[5] += grouped.Count;
                                    break;
                            }
                            if (grouped.Count >= 6)
                            {
                                superNova = true;
                            }
                        }
                        grouped.ForEach(x =>
                        {
                            int xi = -1, xj = -1;
                            getIJ(x.GetComponent<GemControler>(), out xi, out xj);
                            board[xi, xj] = null;
                            x.gameObject.SetActive(false);
                        });
                        board[i, j] = null;
                    }
                }
            }
        }

        if (battleState == BattleState.battle)
            DoAttack();
    }

    private void FallGems()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] == null)
                {
                    if (board[i, j + 1] != null)
                    {
                        float x, y;
                        GetXAndYOnBoard(i, j, out x, out y);
                        StartCoroutine(board[i, j + 1].Move(new Vector2(x, y)));
                        if (board[i, j + 1].reached)
                        {
                            board[i, j] = board[i, j + 1];
                            board[i, j + 1] = null;
                            if (!board[i, j].GetComponent<Collider2D>().enabled)
                            {
                                board[i, j].GetComponent<Collider2D>().enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void GetXAndYOnBoard(int i, int j, out float x, out float y)
    {
        x = -5 - 0.7f + (i * (gems[0].GetComponent<BoxCollider2D>().size.x + 0.01f));
        y = 5 + (j * gems[0].GetComponent<BoxCollider2D>().size.y) + 6.47f;
    }

    private bool IsMapFull()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] == null)
                {
                    isMapFull = false;
                    return false;
                }
            }
        }
        isMapFull = true;
        return true;
    }

    private void generateGemOnTop()
    {
        for (int i = 0; i < col; i++)
        {
            if (board[i, row] == null)
            {
                GenerateGem(i, row, 0);
                board[i, row].GetComponent<Collider2D>().enabled = false;
                //float x, y;
                // GetXAndYOnBoard(i, row - 1, out x, out y);
                //StartCoroutine(board[i, row - 1].Move(new Vector2(x, y)));
            }
        }
    }

    private IEnumerator MatchAndDestroy()
    {
        coIsRunning = true;
        yield return new WaitForEndOfFrame();
        CheckForMatch();
        if (finded)
        {
            if (battleState == BattleState.battle)
                yield return new WaitForSeconds(0.7f);
            //Debug.Break();
            yield return new WaitForFixedUpdate();
            while (PauseControler.pause)
            {
                yield return null;
            }
            DestroyMatches();
            finded = false;
        }
        coIsRunning = false;
    }

    private void DoAttack()
    {
        bool isSuperNova = false;
        Color c = new Color();
        //0-attack 1-heal 2-shield 3-buff 4-gold 5-debuff
        if (destroyedGemCount[3] > 0)//buff
        {
            ColorUtility.TryParseHtmlString("#FF8E00FF", out c);
            int value = destroyedGemCount[3] - 2;
            player.CurrBuff += value;

            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
            isSuperNova = isSuperNova ? true : (destroyedGemCount[3] >= superNovaCounter ? true : false);
            audioSource[0].clip = audioClips[3];
        }
        if (destroyedGemCount[5] > 0)//debuff
        {
            ColorUtility.TryParseHtmlString("#FF8E00FF", out c);
            int value = destroyedGemCount[5];
            monster.CurrBuff -= value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), enemyImageTransform, c);
            isSuperNova = isSuperNova ? true : (destroyedGemCount[5] >= superNovaCounter ? true : false);
            audioSource[0].clip = audioClips[4];
        }
        if (destroyedGemCount[4] > 0)//gold
        {
            ColorUtility.TryParseHtmlString("#F1FF00FF", out c);
            int value = ((destroyedGemCount[4] - 3) * 20) + 30;
            player.gold += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
            isSuperNova = isSuperNova ? true : (destroyedGemCount[4] >= superNovaCounter ? true : false);
            audioSource[0].clip = audioClips[5];
        }
        if (destroyedGemCount[1] > 0)//heal
        {
            ColorUtility.TryParseHtmlString("#00FF00FF", out c);
            int value = ((destroyedGemCount[1] - 3) * 2) + 3;
            player.HitPoint += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
            isSuperNova = isSuperNova ? true : (destroyedGemCount[1] >= superNovaCounter ? true : false);
            audioSource[0].clip = audioClips[2];
        }
        if (destroyedGemCount[2] > 0)//shield
        {
            ColorUtility.TryParseHtmlString("#1697C5FF", out c);
            int value = ((destroyedGemCount[2] - 3) * 2) + 3 + player.CurrBuff;
            value = value > 0 ? value : 0;
            player.CurrShield += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
            isSuperNova = isSuperNova ? true : (destroyedGemCount[2] >= superNovaCounter ? true : false);
            audioSource[0].clip = audioClips[1];
        }
        if (destroyedGemCount[0] > 0)//attack
        {
            ColorUtility.TryParseHtmlString("#B60101FF", out c);
            //destroyedGemCount[0] += player.currBuff;
            //destroyedGemCount[0] = destroyedGemCount[0] >= 0 ? destroyedGemCount[0] : 0;
            //int value = ((destroyedGemCount[0] - 3) * 2) + 3;
            int value = 3;
            for (int a = 1; a <= (destroyedGemCount[0] - 3); a++)
            {
                value += a * 2;
            }
            value += player.CurrBuff;
            value = value >= 0 ? value : 0;
            PopoutCreator.CreatePopoutText("-" + value.ToString(), enemyImageTransform, c);
            int temp = monster.CurrShield;
            monster.CurrShield -= value;
            value -= temp;
            if (value > 0)
            {
                monster.CurrHp -= value;
            }
            isSuperNova = isSuperNova ? true : (destroyedGemCount[0] >= superNovaCounter ? true : false);
            audioSource[0].clip = audioClips[0];
        }
        int i = Random.Range(0, 2);
        audioSource[0].Play();
        if (superNova)
        {
            GameObject nova = Instantiate(superNovaAnim);
            nova.transform.SetParent(novaPosition.transform);
            nova.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Destroy(nova, superNovaAnim.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length - 0.1f);
            audioSource[1].clip = audioClips[6];
            audioSource[1].Play();
            player.currShipEnergy += 1;
            superNova = false;
        }

        Array.Clear(destroyedGemCount, 0, destroyedGemCount.Length);
        if (monster.CurrHp <= 0)
        {
            battleState = BattleState.win;
            monster.Kill();
        }
    }

    private void moveTwoGems()
    {
        int i, j, i2, j2;
        Vector2 vec = GemControler.toSwap[0].transform.position;
        StartCoroutine(GemControler.toSwap[0].Move(GemControler.toSwap[1].transform.position));
        StartCoroutine(GemControler.toSwap[1].Move(vec));
        getIJ(GemControler.toSwap[0], out i, out j);
        getIJ(GemControler.toSwap[1], out i2, out j2);
        GemControler temp = board[i, j];
        board[i, j] = board[i2, j2];
        board[i2, j2] = temp;
        GemControler.toSwap.Clear();
        cleared = false;
    }

    private void Update()
    {
        if (battleState == BattleState.creatingMap)
        {
            if (GemControler.anyCoIsRun.Count == 0)
            {
                if (IsMapFull())
                {
                    SetNeightbours();
                    if (!coIsRunning)
                    {
                        if (cleared)
                        {
                            battleState = BattleState.battle;
                        }
                        if (!cleared)
                        {
                            StartCoroutine(MatchAndDestroy());
                            cleared = true;
                        }
                    }
                }
            }
            if (!IsMapFull())
            {
                FallGems();
                generateGemOnTop();
                cleared = false;
            }
        }
        if (battleState == BattleState.battle)
        {
            if (GemControler.anyCoIsRun.Count == 0)
            {
                if (IsMapFull())
                {
                    SetNeightbours();
                    if (!coIsRunning)
                    {
                        if (player.currShipEnergy == 0 && cleared)
                        {
                            if (!monsterSong)
                            {
                                monster.PlayTrump();
                                monsterSong = true;
                            }
                            if (monsterSong && !monster.GetComponent<AudioSource>().isPlaying)
                            {
                                monster.MakeAMove();
                                if (player.HitPoint <= 0)
                                {
                                    battleState = BattleState.lose;
                                }
                                player.currShipEnergy = player.ShipEnergy;
                                monsterSong = false;
                            }
                        }
                        if (!cleared)
                        {
                            StartCoroutine(MatchAndDestroy());
                            cleared = true;
                        }
                        if (GemControler.toSwap.Count == 2)
                        {
                            moveTwoGems();
                            player.currShipEnergy -= 1;
                            player.inventory.ForEach(item => { item.DoOnEveryMove(); });
                        }
                    }
                }
            }
            if (!IsMapFull())
            {
                FallGems();
                generateGemOnTop();
                cleared = false;
            }
        }
    }

    private bool getIJ(GemControler gem, out int xi, out int xj)
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (gem == board[i, j])
                {
                    xi = i;
                    xj = j;
                    return true;
                }
            }
        }
        xi = -1;
        xj = -1;
        return false;
    }
}

public enum BattleState
{
    battle = 0, pause = 1, win = 2, creatingMap = 3, lose = 4
}