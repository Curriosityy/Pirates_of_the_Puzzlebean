using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemControler : MonoBehaviour
{
    public float gemsSpeed;
    private Rigidbody2D rb;
    private Collider2D cd;

    [HideInInspector]
    public bool move = false;

    public ParticleSystem ps;

    public bool matched = false;

    public bool reached = true;

    public static Stack<bool> anyCoIsRun = new Stack<bool>();

    public GameObject[] neighbors = { null, null, null, null };

    // Use this for initialization
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        ps = gameObject.GetComponent<ParticleSystem>();
        cd = gameObject.GetComponent<Collider2D>();
        reached = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (matched)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }

    private void RefreshNeightbors()
    {
    }

    public void SearchForMatch()
    {
        if (!matched)
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
            }
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

    public IEnumerator Move(Vector2 endPoint)
    {
        if (!move)
        {
            anyCoIsRun.Push(true);
            while (this != null && (Vector2)transform.position != endPoint)
            {
                matched = false;
                move = true;
                reached = false;
                //transform.position = Vector2.Lerp(transform.position, endPoint, gemsSpeed * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, endPoint, gemsSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = endPoint;
            move = false;
            reached = true;
            anyCoIsRun.Pop();
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}