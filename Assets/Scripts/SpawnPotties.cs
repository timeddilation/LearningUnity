using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPotties : MonoBehaviour {

    public GameObject potty;
    public int pottiesToSpawn = 0;
    public GameObject pottySpawnLocations;
    public NavMeshSurface surface;

    private int pottiesSpawned;
    private int numberOfPottySpawnLocations;

    // Use this for initialization
    void Start ()
    {
        numberOfPottySpawnLocations = pottySpawnLocations.transform.childCount;
        if (pottiesToSpawn > numberOfPottySpawnLocations) { pottiesToSpawn = numberOfPottySpawnLocations; }

        pottiesSpawned = 0;
        GeneratePotties();
        surface.BuildNavMesh();
    }

    private void GeneratePotties()
    {
        //Potties face negative X by default
        GameObject[] pottyLocationsNX = GameObject.FindGameObjectsWithTag("SpawnPottyNegX");
        foreach (GameObject pottyLocation in pottyLocationsNX)
        {
            if (pottiesSpawned < pottiesToSpawn)
            {
                GameObject go = Instantiate(potty, pottyLocation.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                GameObject goEntrance = go.transform.Find("EntranceTrigger").gameObject;
                SomeoneEntered mything = goEntrance.GetComponent<SomeoneEntered>();
                mything.facingX = -1;
                mything.facingZ = 0;
                pottiesSpawned = ++pottiesSpawned;
            }
        }

        GameObject[] pottyLocationsPX = GameObject.FindGameObjectsWithTag("SpawnPottyPosX");
        foreach (GameObject pottyLocation in pottyLocationsPX)
        {
            if (pottiesSpawned < pottiesToSpawn)
            {
                GameObject go = Instantiate(potty, pottyLocation.gameObject.transform.position, Quaternion.Euler(0, 180, 0));
                GameObject goEntrance = go.transform.Find("EntranceTrigger").gameObject;
                SomeoneEntered mything = goEntrance.GetComponent<SomeoneEntered>();
                mything.facingX = 1;
                mything.facingZ = 0;
                pottiesSpawned = ++pottiesSpawned;
            }
        }
    }
	
}
