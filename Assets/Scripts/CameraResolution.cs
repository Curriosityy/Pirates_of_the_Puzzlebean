using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResolution : MonoBehaviour
{
    public int pixelToUnits = 100;

    // Update is called once per frame
    private void Update()
    {
        GetComponent<Camera>().orthographicSize = Screen.height / pixelToUnits / 2f;
    }
}