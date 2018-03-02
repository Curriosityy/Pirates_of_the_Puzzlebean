using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HildeIfZeroValue : MonoBehaviour
{
    public Slider slider;
    public GameObject fillArea;

    // Update is called once per frame
    private void Update()
    {
        if (slider.value == 0)
        {
            fillArea.SetActive(false);
        }
        else
        {
            fillArea.SetActive(true);
        }
    }
}