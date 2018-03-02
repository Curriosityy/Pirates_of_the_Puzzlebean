using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class BattleControler : MonoBehaviour
{
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

    private void Start()
    {
        coIsRunning = false;
        isMapFull = false;
        cleared = false;
        finded = false;
        board = new GemControler[col, row + 1];
        GenerateBoard();
        player = Player.Instance;
        monster = Monster.Instance;
        destroyedGemCount = new int[gems.Length];
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
        //0-attack 1-heal 2-shield 3-buff 4-gold
        if (destroyedGemCount[0] > 0)
        {
            destroyedGemCount[0] += player.currBuff;
            int temp = monster.CurrShield;
            monster.CurrShield -= destroyedGemCount[0];
            destroyedGemCount[0] -= temp;
            if (destroyedGemCount[0] > 0)
            {
                monster.CurrHp -= destroyedGemCount[0];
            }
        }
        if (destroyedGemCount[1] > 0)
        {
            player.HitPoint += destroyedGemCount[1];
        }
        if (destroyedGemCount[2] > 0)
        {
            player.CurrShield += destroyedGemCount[2] + player.currBuff;
        }
        if (destroyedGemCount[3] > 0)
        {
            player.currBuff += destroyedGemCount[3] - 2;
        }
        if (destroyedGemCount[4] > 0)
        {
            player.gold += destroyedGemCount[4] * 10;
        }
        Array.Clear(destroyedGemCount, 0, destroyedGemCount.Length);
    }

    private void Update()
    {
        if (!PauseControler.pause)
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
                            player.currShipEnergy = player.ShipEnergy;
                        }
                        if (!cleared)
                        {
                            StartCoroutine(MatchAndDestroy());
                            cleared = true;
                        }
                        if (GemControler.toSwap.Count == 2)
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