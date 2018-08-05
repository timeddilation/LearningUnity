using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance;
    public int pottiesToSpawn = 8;
    public int pottiesSpawned = 0;

    public NavMeshSurface surface;
    public GameObject standardPottyPrefab;

    private GameObject pottyToBuild;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
        }
        instance = this;
    }

    //private void Start()
    //{
    //    pottyToBuild = standardPottyPrefab;
    //}

    public GameObject GetPottyToBuild()
    {
        return pottyToBuild;
    }

    public void SetPottyToBuild(GameObject potty)
    {
        pottyToBuild = potty;
    }
}
