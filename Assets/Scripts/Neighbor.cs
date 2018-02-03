using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{
    public GameObject neightbor;
    private List<GameObject> listNeig = new List<GameObject>();

    // Use this for initialization
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && transform.parent.GetInstanceID() != collision.transform.GetInstanceID())
        {
            listNeig.Add(collision.gameObject);
            neightbor = listNeig[listNeig.Count - 1];
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger && transform.parent.GetInstanceID() != collision.transform.GetInstanceID())
        {
            listNeig.Remove(collision.gameObject);
            if (listNeig.Count > 0)
            {
                neightbor = listNeig[listNeig.Count - 1];
            }
            else
            {
                neightbor = null;
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
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

    private void OnTriggerStay2D(Collider2D collision)
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

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && transform.parent.GetInstanceID() != other.transform.GetInstanceID())
        {
            neightbor = other.gameObject;
            switch (name)
            {
                case "Up":
                    {
                        GetComponentInParent<GemControler>().neighbors[1] = null;
                    }
                    break;

                case "Down":
                    {
                        GetComponentInParent<GemControler>().neighbors[3] = null;
                    }
                    break;

                case "Left":
                    {
                        GetComponentInParent<GemControler>().neighbors[0] = null;
                    }
                    break;

                case "Right":
                    {
                        GetComponentInParent<GemControler>().neighbors[2] = null;
                    }
                    break;
            }
        }
    }*/
}