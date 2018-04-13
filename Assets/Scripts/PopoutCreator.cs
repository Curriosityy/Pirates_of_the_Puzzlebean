using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PopoutCreator : MonoBehaviour
{
    private static PopoutControler popout;

    private static Canvas canvas;

    // Use this for initialization
    public static void Initialize()
    {
        popout = Resources.Load<PopoutControler>("PopOutTextParent");
        canvas = FindObjectOfType<Canvas>();
    }

    public static void CreatePopoutText(string xText, Transform xtransform, Color xColor)
    {
        PopoutControler instantiated = Instantiate(popout, xtransform);
        instantiated.transform.rotation = Quaternion.identity;
        Vector3 vec = instantiated.transform.position;
        vec.x += Random.Range(-1f, 1f);
        instantiated.transform.position = vec;
        instantiated.transform.SetParent(canvas.transform);
        instantiated.SetText(xText);
        instantiated.SetColor(xColor);
    }
}