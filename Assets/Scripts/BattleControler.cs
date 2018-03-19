using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class BattleControler : MonoBehaviour
{
    public static BattleState battleState;
    private MapControler mapControler;
    public GameObject[] gems;
    private int[] destroyedGemCount;//0-attack 1-heal 2-shield 3-buff 4-gold
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

    private void Start()
    {
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
        player.currBuff = 0;
        player.CurrShield = 0;
        player.ShieldMax = 0;
        PopoutCreator.Initialize();
        mapControler = GameObject.Find("MapControler").GetComponent<MapControler>();
        playerImageTransform = GameObject.Find("PlayerImage").transform;
        enemyImageTransform = GameObject.Find("EnemyImage").transform;
    }

    private void GenerateBoard()
    {
        boardHolder = new GameObject("board").transform;
        boardHolder.transform.position = new Vector2(boardHolder.position.x, 6.47f);
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GenerateGem(i, j, 0);
            }
        }
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
                    checkForMatch(board[i, j]);
                }
            }
        }
    }

    private void checkForMatch(GemControler xgemControler)
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
                        board[i, j].gameObject.SetActive(false);
                        if (battleState == BattleState.battle)
                            switch (board[i, j].gameObject.tag)
                            {
                                case "gem6":
                                    destroyedGemCount[0] += 1;
                                    break;

                                case "gem3":
                                    destroyedGemCount[1] += 1;
                                    break;

                                case "gem4":
                                    destroyedGemCount[2] += 1;
                                    break;

                                case "gem1":
                                    destroyedGemCount[3] += 1;
                                    break;

                                case "gem2":
                                    destroyedGemCount[4] += 1;
                                    break;
                            }
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
        x = -5 + (i * (gems[0].GetComponent<BoxCollider2D>().size.x + 0.1f));
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
        yield return new WaitForFixedUpdate();
        CheckForMatch();
        if (finded)
        {
            if (battleState == BattleState.battle)
                yield return new WaitForSeconds(.5f);
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
        Color c = new Color();
        //0-attack 1-heal 2-shield 3-buff 4-gold
        if (destroyedGemCount[3] > 0)
        {
            ColorUtility.TryParseHtmlString("#FF8E00FF", out c);
            int value = destroyedGemCount[3] - 2;
            player.currBuff += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
        }
        if (destroyedGemCount[4] > 0)
        {
            ColorUtility.TryParseHtmlString("#F1FF00FF", out c);
            int value = destroyedGemCount[4] * 10;
            player.gold += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
        }
        if (destroyedGemCount[0] > 0)
        {
            ColorUtility.TryParseHtmlString("#B60101FF", out c);
            destroyedGemCount[0] += player.currBuff;
            destroyedGemCount[0] = destroyedGemCount[0] >= 0 ? destroyedGemCount[0] : 0;
            int value = destroyedGemCount[0];
            int temp = monster.CurrShield;
            monster.CurrShield -= destroyedGemCount[0];
            destroyedGemCount[0] -= temp;
            if (destroyedGemCount[0] > 0)
            {
                monster.CurrHp -= destroyedGemCount[0];
            }
            PopoutCreator.CreatePopoutText("-" + value.ToString(), enemyImageTransform, c);
        }
        if (destroyedGemCount[1] > 0)
        {
            ColorUtility.TryParseHtmlString("#00FF00FF", out c);
            int value = destroyedGemCount[1];
            player.HitPoint += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
        }
        if (destroyedGemCount[2] > 0)
        {
            ColorUtility.TryParseHtmlString("#1697C5FF", out c);
            int value = destroyedGemCount[2] + player.currBuff;
            value = value > 0 ? value : 0;
            player.CurrShield += value;
            PopoutCreator.CreatePopoutText("+" + value.ToString(), playerImageTransform, c);
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
                    if (!coIsRunning)
                    {
                        if (player.currShipEnergy == 0 && cleared)
                        {
                            monster.MakeAMove();
                            if (player.HitPoint <= 0)
                            {
                                battleState = BattleState.lose;
                            }
                            player.currShipEnergy = player.ShipEnergy;
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