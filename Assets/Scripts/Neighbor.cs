using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{
    public GameObject neightbor;

    // Use this for initialization
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && transform.parent.GetInstanceID() != collision.transform.GetInstanceID())
        {
            neightbor = collision.gameObject;
            switch (name)
            {
                case "Up":
                    {
                        GetComponentInParent<GemControler>().neighbors[1] = neightbor;
                    }
                    break;

                case "Down":
                    {
                        GetComponentInParent<GemControler>().neighbors[3] = neightbor;
                    }
                    break;

                case "Left":
                    {
                        GetComponentInParent<GemControler>().neighbors[0] = neightbor;
                    }
                    break;

                case "Right":
                    {
                        GetComponentInParent<GemControler>().neighbors[2] = neightbor;
                    }
                    break;
            }
        }
    }

    private void deleteIfEqual(GameObject go, int i)
    {
        if (GetComponentInParent<GemControler>().neighbors[i].GetInstanceID() == go.GetInstanceID())
        {
            GetComponentInParent<GemControler>().neighbors[i] = null;
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger && transform.parent.GetInstanceID() != collision.transform.GetInstanceID())
        {
            switch (name)
            {
                case "Up":
                    {
                        deleteIfEqual(collision.gameObject, 1);
                    }
                    break;

                case "Down":
                    {
                        deleteIfEqual(collision.gameObject, 3);
                    }
                    break;

                case "Left":
                    {
                        deleteIfEqual(collision.gameObject, 0);
                    }
                    break;

                case "Right":
                    {
                        deleteIfEqual(collision.gameObject, 2);
                    }
                    break;
            }
        }
    }*/

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDestroy()
    {
    }
}