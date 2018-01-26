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
                board[i, j] = Instantiate(gems[Random.Range(0, gems.Length)], new Vector3(-5 + (i * (gems[0].GetComponent<BoxCollider2D>().size.x + 0.1f)), 5 + (j * gems[0].GetComponent<BoxCollider2D>().size.y), 0), Quaternion.identity);
                board[i, j].transform.SetParent(boardHolder);
            }
        }
        StartCoroutine(RefreshOnStart());
    }

    private IEnumerator RefreshOnStart()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < boardHolder.childCount; i++)
        {
            RefreshNeighbor(boardHolder.GetChild(i).GetComponent<GemControler>());
        }
    }

    private void RefreshNeighbor(GemControler xgemControler)
    {
        xgemControler.RefreshNeighbor();
        checkForMatch(xgemControler);
    }

    private void checkForMatch(GemControler xgemControler)
    {
        xgemControler.SearchForMatch();
        destroyMatched();
    }

    private void destroyMatched()
    {
        for (int i = 0; i < boardHolder.childCount; i++)
        {
            if (boardHolder.GetChild(i).GetComponentInChildren<GemControler>().matched)
                Destroy(boardHolder.GetChild(i).gameObject);
        }
    }

    private void FixedUpdate()
    {
    }

    // Update is called once per frame

    private void Update()
    {
    }
}