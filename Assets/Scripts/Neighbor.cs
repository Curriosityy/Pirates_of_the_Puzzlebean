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

    private void OnTriggerExit(Collider other)
    {
        // listNeig.Remove(other.gameObject);
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
}