using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && transform.parent.GetInstanceID() != collision.transform.GetInstanceID())
        {
            switch (name)
            {
                case "Up":
                    {
                        GetComponentInParent<GemControler>().neighbors[1] = collision.gameObject;
                    }
                    break;

                case "Down":
                    {
                        GetComponentInParent<GemControler>().neighbors[3] = collision.gameObject;
                    }
                    break;

                case "Left":
                    {
                        GetComponentInParent<GemControler>().neighbors[0] = collision.gameObject;
                    }
                    break;

                case "Right":
                    {
                        GetComponentInParent<GemControler>().neighbors[2] = collision.gameObject;
                    }
                    break;
            }
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

    // Update is called once per frame
    private void Update()
    {
    }
}