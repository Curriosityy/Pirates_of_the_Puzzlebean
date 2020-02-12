using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemControler : MonoBehaviour
{
    public float gemsSpeed;
    private float tempSpeed;

    public Animator explosionAnimation;

    [HideInInspector]
    public bool move = false;

    public ParticleSystem ps;

    public bool matched = false;

    public bool reached = true;

    public bool selected = false;

    public static Stack<bool> anyCoIsRun = new Stack<bool>();

    public GameObject[] neighbors = { null, null, null, null };

    public static List<GemControler> toSwap = new List<GemControler>();

    // Use this for initialization
    private void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody2D>();
        tempSpeed = gemsSpeed;
        reached = true;
        ps = gameObject.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (neighbors[i] != null)
                Debug.DrawLine(transform.position, neighbors[i].transform.position, Color.red);
        }
        if (BattleControler.battleState == BattleState.creatingMap)
        {
            gemsSpeed = 10000;
        }
        else
        {
            gemsSpeed = tempSpeed;
        }
        if (selected || matched)
        {
            ps.Play();
        }
        else
        {
            if (!matched)
            {
                ps.Stop();
            }
        }
    }

    private void GetNeighbors()
    {
        //neighbors[1] = transform.Find("Up").GetComponent<Neighbor>().neightbor;
        //neighbors[3] = transform.Find("Down").GetComponent<Neighbor>().neightbor;
        //neighbors[0] = transform.Find("Left").GetComponent<Neighbor>().neightbor;
        //neighbors[2] = transform.Find("Right").GetComponent<Neighbor>().neightbor;
    }

    public List<GameObject> GroupMatched()
    {
        List<GameObject> group = new List<GameObject>();
        Queue<GemControler> bfsQueue = BFS(this);
        while (bfsQueue.Count > 0)
        {
            if (!group.Contains(bfsQueue.Peek().gameObject))
            {
                group.Add(bfsQueue.Peek().gameObject);
                Queue<GemControler> temp = BFS(bfsQueue.Dequeue());
                while (temp.Count > 0)
                {
                    bfsQueue.Enqueue(temp.Dequeue());
                }
            }
            else
            {
                bfsQueue.Dequeue();
            }
        }
        return group;
    }

    public Queue<GemControler> BFS(GemControler gc)
    {
        Queue<GemControler> lookFor = new Queue<GemControler>();
        for (int i = 0; i < 4; i++)
        {
            if (gc.neighbors[i] != null)
                if (gc.neighbors[i].tag == gc.tag && gc.neighbors[i].GetComponent<GemControler>().matched)
                {
                    lookFor.Enqueue(gc.neighbors[i].GetComponent<GemControler>());
                }
        }
        return lookFor;
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
                BattleControler.finded = true;
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
            selected = false;
            anyCoIsRun.Pop();
        }
    }

    private void OnDisable()
    {
        if (BattleControler.battleState == BattleState.battle)
        {
            Vector3 pos = transform.position;
            pos.z = pos.z - 2;
            GameObject explo = Instantiate(explosionAnimation.gameObject, pos, Quaternion.identity);
            Destroy(explo, explosionAnimation.runtimeAnimatorController.animationClips[0].length - 0.1f);
        }
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (GameObject.FindGameObjectWithTag("InfoPanel") == null && GameObject.FindGameObjectWithTag("PausePanel") == null)
        {
            if (BattleControler.battleState == BattleState.battle)
            {
                if (toSwap.Count < 2 && anyCoIsRun.Count == 0 && BattleControler.isMapFull && !BattleControler.coIsRunning && Player.Instance.currShipEnergy > 0)
                {
                    if (!toSwap.Contains(this))
                    {
                        if (toSwap.Count == 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (toSwap[0].neighbors[i] == gameObject)
                                {
                                    toSwap.Add(this);
                                    selected = true;
                                }
                            }
                        }
                        else
                        {
                            toSwap.Add(this);
                            selected = true;
                        }
                    }
                    else
                    {
                        toSwap.Remove(this);
                        selected = false;
                    }
                }
            }
        }
    }
}