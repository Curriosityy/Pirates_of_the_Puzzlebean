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
                GameObject instance = Instantiate(gems[Random.Range(0, gems.Length)], new Vector3(-5 + i * gems[0].GetComponent<BoxCollider2D>().size.x, 5 + j * gems[0].GetComponent<BoxCollider2D>().size.y, 0), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}