using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSwim : MonoBehaviour
{
    public float swimSpeed;
    public float timeToWait;
    private static bool beingHandled = false;
    private BuoyancyEffector2D be2d;

    // Use this for initialization
    private void Start()
    {
        be2d = GetComponent<BuoyancyEffector2D>();
    }

    private IEnumerator changeVelocity()
    {
        beingHandled = true;
        be2d.flowVariation = swimSpeed;
        yield return new WaitForSeconds(timeToWait);
        be2d.flowVariation = -swimSpeed;
        yield return new WaitForSeconds(timeToWait);
        beingHandled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!beingHandled)
        {
            StartCoroutine(changeVelocity());
        }
    }
}