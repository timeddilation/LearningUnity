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

    void Start ()
    {
        GameObject[] pottyLocations = GameObject.FindGameObjectsWithTag("SpawnPotty");
        int numberOfPottySpawnLocations = pottyLocations.Length;
        pottiesSpawned = 0;
        if (pottiesToSpawn > numberOfPottySpawnLocations) { pottiesToSpawn = numberOfPottySpawnLocations; }

        GeneratePotties(pottyLocations);
        InformGameManagerOfPotties();
        surface.BuildNavMesh();
    }

    private void GeneratePotties(GameObject[] pottyLocations)
    {
        foreach (GameObject pottyLocation in pottyLocations)
        {
            portaSpotData locationData = pottyLocation.gameObject.GetComponent<portaSpotData>();
            float spawnRotation = GetRotationOfPotty(locationData.facingX, locationData.facingZ);

            if (pottiesSpawned < pottiesToSpawn)
            {
                GameObject go = Instantiate(potty, pottyLocation.gameObject.transform.position, Quaternion.Euler(0, spawnRotation, 0));
                GameObject goEntrance = go.transform.Find("EntranceTrigger").gameObject;
                SomeoneEntered pottySetup = goEntrance.GetComponent<SomeoneEntered>();
                pottySetup.facingX = locationData.facingX;
                pottySetup.facingZ = locationData.facingZ;
                locationData.hasPotty = true;
                pottiesSpawned = ++pottiesSpawned;
            }
        }
    }

    private float GetRotationOfPotty(int facingX, int facingZ)
    {
        float rotationValue = 0f;
        if (facingX < 0) { rotationValue = 0f; }
        else if (facingX > 0) { rotationValue = 180f; }
        else if (facingZ < 0) { rotationValue = 90f; }
        else if (facingZ > 0) { rotationValue = -90f; }
        return rotationValue;
    }
	
    private void InformGameManagerOfPotties()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        DebugMenu debugPotties = gameManager.GetComponent<DebugMenu>();

        GameObject[] allPotties = GameObject.FindGameObjectsWithTag("Potty");
        debugPotties.numberOfPotties = allPotties.Length;

        WorldValuesAndObjects.availablePotties = GameObject.FindGameObjectsWithTag("Potty");
    }
}
