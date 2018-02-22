using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaver : MonoBehaviour
{
    private static MapSaver instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}