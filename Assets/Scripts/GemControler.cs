using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemControler : MonoBehaviour
{
    [HideInInspector]
    public bool move = false;

    public bool matched = false;

    public GameObject[] neighbors = { null, null, null, null };

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //RefreshNeighbor();
    }

    public bool SearchNeighbors()
    {
        List<GameObject> vertList = new List<GameObject>();
        List<GameObject> horiList = new List<GameObject>();
        List<GameObject> list = new List<GameObject>();
        list.Add(gameObject);
        for (int i = 0; i < 4; i++)
        {
            if (i % 2 == 0)
            {
                Stack<GameObject> stack = SearchIn(i);
                while (stack.Count > 0)
                {
                    vertList.Add(stack.Pop());
                }
            }
            else
            {
                Stack<GameObject> stack = SearchIn(i);
                while (stack.Count > 0)
                {
                    horiList.Add(stack.Pop());
                }
            }
        }
        if (vertList.Count >= 2)
            list.AddRange(vertList);
        if (horiList.Count >= 2)
            list.AddRange(horiList);
        if (list.Count >= 3)
        {
            foreach (GameObject g in list)
            {
                g.GetComponent<GemControler>().matched = true;
            }
            list.Clear();
            return true;
        }
        else
        {
            return false;
        }
    }

    private Stack<GameObject> SearchIn(int i)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        GameObject neighbor;
        if (neighbors[i] != null)
        {
            neighbor = neighbors[i];

            while (tag == neighbor.tag)
            {
                stack.Push(neighbor);
                neighbor = stack.Peek().transform.GetComponent<GemControler>().neighbors[i];
                if (stack.Peek() == neighbor || neighbor == null)
                {
                    break;
                }
            }
        }
        return stack;
    }

    public void RefreshNeighbor()
    {
        neighbors[0] = transform.Find("Left").GetComponent<Neighbor>().neightbor;
        neighbors[2] = transform.Find("Right").GetComponent<Neighbor>().neightbor;
        neighbors[1] = transform.Find("Up").GetComponent<Neighbor>().neightbor;
        neighbors[3] = transform.Find("Down").GetComponent<Neighbor>().neightbor;
    }
}