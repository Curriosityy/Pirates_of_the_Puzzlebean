using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapControler : MonoBehaviour
{
    public GameObject previousCross;
    public GameObject selectedCross;
    public GameObject mapHolder;
    public Enemy[] listOfEnemy;
    public double[] enemyProbability;
    public GameObject monster;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("halo");
    }

    private Enemy RandAEnemy()
    {
        float rand = Random.Range(0f, 1f);
        double sumOfRand = 0;
        for (int i = 0; i < listOfEnemy.Length; i++)
        {
            sumOfRand += enemyProbability[i];
            if (rand < sumOfRand)
            {
                Debug.Log(rand);
                return listOfEnemy[i];
            }
        }
        return listOfEnemy[listOfEnemy.Length - 1];
    }

    private void OnMouseDown()
    {
        if (previousCross == null)
        {
            GameObject initializedMonster = Instantiate(monster, Vector3.zero, Quaternion.identity);
            initializedMonster.GetComponent<Monster>().Initialize(RandAEnemy());
            Instantiate(selectedCross, transform.position, Quaternion.identity, mapHolder.transform);
            Destroy(gameObject);
            MenuControler.ChangeBetweenScenes(2);
            mapHolder.SetActive(false);
        }
    }
}