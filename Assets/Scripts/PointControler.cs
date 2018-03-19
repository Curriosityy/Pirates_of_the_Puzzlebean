using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointControler : MonoBehaviour
{
    public bool isVisited = false;
    public IPointStrategyType pst;
    private MapControler mapControler;
    public MapControler.Point thisPoint;
    private GameObject visited;

    // Use this for initialization
    private void Start()
    {
        mapControler = GameObject.Find("MapControler").GetComponent<MapControler>();
        switch (tag)
        {
            case "normal":
                pst = new NormalPoint();
                break;

            case "boss":
                pst = new BossPoint();
                break;

            case "shop":
                pst = new ShopPoint();
                break;

            case "elite":
                pst = new ElitePoint();
                break;

            case "rest":
                pst = new RestPoint();
                break;
        }

        visited = transform.GetChild(0).gameObject;
        visited.SetActive(isVisited);
    }

    // Update is called once per frame
    private void Update()
    {
        visited.SetActive(isVisited);
    }

    private void OnMouseDown()
    {
        if (mapControler.pointOfStaying.IsConnectedTo(thisPoint))
        {
            mapControler.currLevel += 1;
            mapControler.pointToGo = thisPoint;
            pst.DoWhenClicked();
            if (tag == "shop" || tag == "rest")
            {
                mapControler.pointOfStaying = mapControler.pointToGo;
                isVisited = true;
            }
        }
    }
}