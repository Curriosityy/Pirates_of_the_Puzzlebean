using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemControler : MonoBehaviour
{
    [HideInInspector]
    public bool move = false;

    public GameObject[] neighbors = new GameObject[4];

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //RefreshNeighbor();
    }

    public void RefreshNeighbor()
    {
        neighbors[0] = transform.Find("Left").GetComponent<Neighbor>().neightbor;
        neighbors[1] = transform.Find("Right").GetComponent<Neighbor>().neightbor;
        neighbors[2] = transform.Find("Up").GetComponent<Neighbor>().neightbor;
        neighbors[3] = transform.Find("Down").GetComponent<Neighbor>().neightbor;
    }
}