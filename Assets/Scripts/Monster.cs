using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private static Monster instance;
    private Enemy enemy;

    public static Monster Instance
    {
        get
        {
            return instance;
        }
    }

    public void Initialize()
    {
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
            Destroy(gameObject.transform);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}