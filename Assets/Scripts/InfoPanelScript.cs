using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelScript : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    public void OnButtonClick()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}