﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleControler : MonoBehaviour
{
    public GameObject[] gems;
    public int row;
    public int col;
    private GemControler[,] board;
    public bool finded = false;
    private Transform boardHolder;
    public float[] probabilityOfEachGem;
    private static bool coIsRunning = false;

    private void Start()
    {
        board = new GemControler[col, row + 1];
        GenerateBoard();
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
                    if (board[i, j].matched && !board[i, j].move)
                    {
                        Debug.Log(board[i, j].tag);
                        board[i, j].gameObject.SetActive(false);
                        board[i, j] = null;
                    }
            }
        }
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

    private bool isMapFull()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] == null)
                {
                    return false;
                }
            }
        }
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
        CheckForMatch();
        yield return null;
        DestroyMatches();
        coIsRunning = false;
    }

    private void Update()
    {
        if (isMapFull())
        {
            /*CheckForMatch();
            DestroyMatches();
            */
            if (!coIsRunning)
            {
                StartCoroutine(MatchAndDestroy());
            }
        }
        if (!isMapFull())
        {
            FallGems();
            generateGemOnTop();
        }
    }
}