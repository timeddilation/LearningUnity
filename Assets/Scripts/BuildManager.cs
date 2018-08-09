using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance;
    public NavMeshSurface surface;
    private WorldValuesAndObjects worldValues;

    public int pottiesToSpawn = 8;
    public int pottiesSpawned = 0;

    public bool hasEnoughMoneyToPurchase = false;    
    //Standard potty prefab setting
    public GameObject standardPottyPrefab;
    public PottyData standardPottyCosts;
    //Standard handicap prefab setting
    public GameObject standardPottyHandiPrefab;
    public PottyData standardPottyHandiCosts;

    public GameObject pottyToBuild;
    public PottyData pottyToBuildCosts;
    
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
        worldValues = WorldValuesAndObjects.instance;

        standardPottyCosts = standardPottyPrefab.GetComponent<PottyData>();
        standardPottyHandiCosts = standardPottyHandiPrefab.GetComponent<PottyData>();
    }

    public GameObject GetPottyToBuild()
    {
        return pottyToBuild;
    }

    public void SetPottyToBuild(GameObject potty)
    {
        pottyToBuild = potty;
        pottyToBuildCosts = potty.GetComponent<PottyData>();
        if (pottyToBuildCosts.cost > worldValues.amountOfMoney)
        {
            hasEnoughMoneyToPurchase = false;
        }
        else
        {
            hasEnoughMoneyToPurchase = true;
        }
    }
}
