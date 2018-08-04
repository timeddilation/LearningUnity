using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSims : MonoBehaviour {

    public GameObject simToSpawn;

    private void Start()
    {
        GameObject[] simSpawnPoints = GameObject.FindGameObjectsWithTag("SpawningSim");
        foreach(GameObject spawnPoint in simSpawnPoints)
        {
            Instantiate(simToSpawn, spawnPoint.transform);
        }
    }
}
