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
        GameObject[] pottyLocations = GameObject.FindGameObjectsWithTag("SpawnPotty");
        numberOfPottySpawnLocations = pottyLocations.Length;
        if (pottiesToSpawn > numberOfPottySpawnLocations) { pottiesToSpawn = numberOfPottySpawnLocations; }

        pottiesSpawned = 0;
        GeneratePotties(pottyLocations);
        surface.BuildNavMesh();
    }

    private void GeneratePotties(GameObject[] pottyLocations)
    {
        foreach (GameObject pottyLocation in pottyLocations)
        {
            portaSpotData locationData = pottyLocation.gameObject.GetComponent<portaSpotData>();
            float spawnRotation = GetRotationOfPotty(locationData.facingX, locationData.facingY);

            if (pottiesSpawned < pottiesToSpawn)
            {
                GameObject go = Instantiate(potty, pottyLocation.gameObject.transform.position, Quaternion.Euler(0, spawnRotation, 0));
                GameObject goEntrance = go.transform.Find("EntranceTrigger").gameObject;
                SomeoneEntered mything = goEntrance.GetComponent<SomeoneEntered>();
                mything.facingX = locationData.facingX;
                mything.facingZ = locationData.facingY;
                pottiesSpawned = ++pottiesSpawned;
            }
        }
    }

    private float GetRotationOfPotty(int facingX, int facingY)
    {
        float rotationValue = 0f;
        if (facingX < 0) { rotationValue = 0f; }
        else if (facingX > 0) { rotationValue = 180f; }
        else if (facingY < 0) { rotationValue = 90f; }
        else if (facingY > 0) { rotationValue = -90f; }
        return rotationValue;
    }
	
}
