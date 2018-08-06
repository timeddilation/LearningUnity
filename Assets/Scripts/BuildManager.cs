using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance;
    public int pottiesToSpawn = 8;
    public int pottiesSpawned = 0;

    public bool hasEnoughMoneyToPurchase = false;

    public NavMeshSurface surface;
    public GameObject standardPottyPrefab;
    public PottyData standardPottyCosts;

    public GameObject pottyToBuild;
    private WorldValuesAndObjects worldValues;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
        }
        instance = this;
    }

    private void Start()
    {
        standardPottyCosts = standardPottyPrefab.GetComponent<PottyData>();
        worldValues = WorldValuesAndObjects.instance;
    }

    public GameObject GetPottyToBuild()
    {
        return pottyToBuild;
    }

    public void SetPottyToBuild(GameObject potty)
    {
        pottyToBuild = potty;
        PottyData pottyCosts = potty.GetComponent<PottyData>();
        if (pottyCosts.cost > worldValues.amountOfMoney)
        {
            hasEnoughMoneyToPurchase = false;
        }
        else
        {
            hasEnoughMoneyToPurchase = true;
        }
    }
}
