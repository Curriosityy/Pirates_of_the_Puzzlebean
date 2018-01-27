using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleControler : MonoBehaviour
{
    public GameObject[] gems;
    public int row;
    public int col;
    private GameObject[,] board;
    public bool finded = false;
    private Transform boardHolder;
    private bool anyMoveObj = false;

    private void Start()
    {
        board = new GameObject[col, row];
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        boardHolder = new GameObject("board").transform;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GenerateGem(i, j);
            }
        }
        StartCoroutine(RefreshOnStart());
    }

    private void GenerateGem(int i, int j)
    {
        board[i, j] = Instantiate(gems[Random.Range(0, gems.Length)], new Vector2(-5 + (i * (gems[0].GetComponent<BoxCollider2D>().size.x + 0.1f)), 5 + (j * gems[0].GetComponent<BoxCollider2D>().size.y)), Quaternion.identity);
        board[i, j].transform.SetParent(boardHolder);
    }

    private IEnumerator RefreshOnStart()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshNeightborsAndCheck();
        destroyMatched();
    }

    private void RefreshNeighbor(GemControler xgemControler)
    {
        xgemControler.RefreshNeighbor();
    }

    private void RefreshNeightborsAndCheck()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] != null)
                {
                    RefreshNeighbor(board[i, j].GetComponent<GemControler>());
                }
            }
        }
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] != null)
                {
                    checkForMatch(board[i, j].GetComponent<GemControler>());
                }
            }
        }
    }

    private void checkForMatch(GemControler xgemControler)
    {
        xgemControler.SearchForMatch();
        //destroyMatched();
    }

    private void destroyMatched()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (board[i, j] != null)
                    if (board[i, j].GetComponent<GemControler>().matched && !board[i, j].GetComponent<GemControler>().move)
                    {
                        board[i, j].SetActive(false);
                        board[i, j] = null;
                    }
            }
        }
    }

    private void FallGems()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row - 1; j++)
            {
                if (board[i, j] == null)
                {
                    if (board[i, j + 1] != null)
                    {
                        //if (!board[i, j + 1].GetComponent<GemControler>().move)
                        StartCoroutine(board[i, j + 1].GetComponent<GemControler>().Move(new Vector2(-5 + (i * (gems[0].GetComponent<BoxCollider2D>().size.x + 0.1f)), 5 + (j * gems[0].GetComponent<BoxCollider2D>().size.y))));
                        if (board[i, j + 1].GetComponent<GemControler>().reached)
                        {
                            board[i, j] = board[i, j + 1];
                            board[i, j + 1] = null;
                        }
                    }
                }
                if (board[i, row - 1] == null)
                {
                    GenerateGem(i, row - 1);
                }
            }
        }
        if (!PauseUntilFilledAndNoMoveingObj())
        {
            RefreshNeightborsAndCheck();
            //Debug.Break();
            destroyMatched();
        }
    }

    private void FixedUpdate()
    {
    }

    // Update is called once per frame
    private bool PauseUntilFilledAndNoMoveingObj()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row - 1; j++)
            {
                if (board[i, j] == null || board[i, j].GetComponent<GemControler>().move || !board[i, j].GetComponent<GemControler>().reached)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Update()
    {
        FallGems();
    }
}