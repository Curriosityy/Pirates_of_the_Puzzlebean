using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemControler : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> neighbors;

    [HideInInspector]
    private bool isSelected = false;

    private void Awake()
    {
        neighbors = new List<GameObject>();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.gameObject.tag != "Table")
        {
            neighbors.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.gameObject.tag != "Table")
        {
            neighbors.Remove(collision.gameObject);
        }
    }
}