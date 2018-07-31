using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectables : MonoBehaviour {

    public GameObject collectables;
    public GameObject[] newCollectables;
    public GameObject goal;
    public GameObject goalPlacement;

    private float minDist;
    private float maxDist;

    void Start()
    {
        minDist = -9f;
        maxDist = 9f;

        for (int i = 0; i < 12; i++)
        {
            newCollectables[i] = Instantiate(collectables, new Vector3(Random.Range(minDist, maxDist), 0.75f, Random.Range(minDist, maxDist)), Quaternion.Euler(45, 45, 45)) as GameObject;
        }

        goalPlacement = Instantiate(goal, new Vector3(-10f, 2f, -10f), Quaternion.Euler(0, 0, 0));
    }

    void spawnCollectables()
    {

        
    }
}
