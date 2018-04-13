using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float edgeSize;
    public float cameraSpeed;
    private Transform mainCamTransform;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector2 mousPos = Input.mousePosition;
        if (Vector2.Distance(mousPos, new Vector2(Screen.width, mousPos.y)) < edgeSize)
        {
            if (mainCamTransform.position.x < 30f)
            {
                mainCamTransform.position = mainCamTransform.position + (Vector3.right * cameraSpeed * Time.deltaTime);
            }
        }
        if (Vector2.Distance(mousPos, new Vector2(0, mousPos.y)) < edgeSize)
        {
            if (mainCamTransform.position.x > -1.5f)
            {
                mainCamTransform.position = mainCamTransform.position + (Vector3.left * cameraSpeed * Time.deltaTime);
            }
        }
    }
}