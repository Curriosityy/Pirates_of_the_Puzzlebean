using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        if (MapSaver.instance != null)
        {
            Destroy(MapSaver.instance.gameObject);
        }
        if (Player.Instance != null)
        {
            Destroy(Player.Instance.gameObject);
        }
        if (Monster.Instance != null)
        {
            Destroy(Monster.Instance.gameObject);
        }
        Destroy(GameObject.Find("MapControler"));
    }

    // Update is called once per frame
    private void Update()
    {
    }
}