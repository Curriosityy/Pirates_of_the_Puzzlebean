using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleControler : MonoBehaviour
{
    public GameObject[] gems;
    public int row;
    public int col;

    private Transform boardHolder;

    private void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        boardHolder = new GameObject("board").transform;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GameObject instance = Instantiate(gems[Random.Range(0, gems.Length)], new Vector3(-5 + i * gems[0].GetComponent<BoxCollider2D>().size.x + 0.1f, 5 + j * gems[0].GetComponent<BoxCollider2D>().size.y + 0.1f, 0), Quaternion.identity);
                instance.transform.SetParent(boardHolder); ;
            }
        }
        StartCoroutine(RefreshNeighbor());
    }

    private IEnumerator RefreshNeighbor()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < boardHolder.childCount; i++)
        {
            boardHolder.GetChild(i).GetComponent<GemControler>().RefreshNeighbor();
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