using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawns : MonoBehaviour {

    public GameObject[] commonRocks;
    public GameObject[] commonRockPlacements;

    public GameObject[] uncommonRocks;
    public GameObject[] uncommonRockPlacements;

    public GameObject[] rareRocks;
    public GameObject[] rareRockPlacements;

    private readonly float minDist = -10f;
    private readonly float maxDist = 10f;
    private readonly float minSpin = 0;
    private readonly float maxSpin = 360f;
    private readonly float minHeight = -0.25f;
    private readonly float maxHeight = 0f;

    // Use this for initialization
    void Start()
    {
        GenerateRocksOnPlane(commonRocks, commonRockPlacements);
        GenerateRocksOnPlane(uncommonRocks, uncommonRockPlacements);
        GenerateRocksOnPlane(rareRocks, rareRockPlacements);
    }

    void GenerateRocksOnPlane(GameObject[] rocks, GameObject[] rockPlacements)
    {
        int rockTypes = rocks.Length;
        int spawnsAvailable = rockPlacements.Length;
        int placementIndex = 0;

        if (rockTypes > 0)
        {
            for (int i = 0; i < rockTypes; i++)
            {
                int currentRockType = i;
                int availableSpots = spawnsAvailable / rockTypes;
                for (int j = 0; j < availableSpots; j++)
                {
                    rockPlacements[placementIndex] = Instantiate(rocks[currentRockType], 
                        new Vector3
                        (
                            Random.Range(minDist, maxDist), 
                            Random.Range(minHeight, maxHeight), 
                            Random.Range(minDist, maxDist)
                        ), 
                        Quaternion.Euler
                        (
                            0, 
                            Random.Range(minSpin, maxSpin), 
                            0
                        ));
                }
            }
        }
    }
}
