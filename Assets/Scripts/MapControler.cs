using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControler : MonoBehaviour
{
    public GameObject previousCross;
    public GameObject selectedCross;
    public GameObject mapHolder;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("halo");
    }

    private void OnMouseDown()
    {
        if (previousCross == null)
        {
            Instantiate(selectedCross, transform.position, Quaternion.identity, mapHolder.transform);
            Destroy(gameObject);
            MenuControler.ChangeBetweenScenes(2);
            mapHolder.SetActive(false);
        }
    }
}