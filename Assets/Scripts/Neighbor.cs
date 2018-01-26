using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{
    public GameObject neightbor = null;

    // Use this for initialization
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && transform.parent.GetInstanceID() != collision.transform.GetInstanceID())
        {
            neightbor = collision.gameObject;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}