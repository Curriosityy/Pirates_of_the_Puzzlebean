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
    private MapPauseControler pauseControler;
    private Animation animation;

    // Use this for initialization
    private void Start()
    {
        animation = gameObject.GetComponent<Animation>();
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

    private void OnMouseOver()
    {
        if (mapControler.PointOfStaying.GetConnection().Contains(thisPoint))
        {
            if (!animation.isPlaying)
            {
                animation.Play();
            }
        }
    }

    private void OnMouseExit()
    {
        animation["PointAnimation"].time = 0.0f;
        animation.Sample();
        animation.Stop();
    }

    private void OnMouseDown()
    {
        if (pauseControler == null)
        {
            pauseControler = Transform.FindObjectOfType<MapPauseControler>();
        }

        if (pauseControler.mapState == MapPauseControler.MapState.map)
        {
            if (mapControler.PointOfStaying.IsConnectedTo(thisPoint))
            {
                mapControler.currLevel += 1;
                mapControler.pointToGo = thisPoint;
                pst.DoWhenClicked();
                if (tag == "shop" || tag == "rest")
                {
                    mapControler.PointOfStaying = mapControler.pointToGo;
                    isVisited = true;
                }
            }
        }
    }
}