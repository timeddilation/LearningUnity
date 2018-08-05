using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueUp : MonoBehaviour {

    public List<GameObject> portaSpots;
    public bool allOccupied;
    public int availablePottiesCount;
    public List<Guid> queuedSims = new List<Guid>();

    private void Start()
    {
        foreach (GameObject portaSpot in portaSpots)
        {
            portaSpotData spotData = portaSpot.gameObject.GetComponent<portaSpotData>();
            spotData.queuePoint = gameObject;
        }
    }

    private void Update()
    {
        List<bool> occupiedPotties = new List<bool>();

        foreach (GameObject portaSpot in portaSpots)
        {
            portaSpotData spotData = portaSpot.gameObject.GetComponent<portaSpotData>();   
            if (spotData.hasPotty) { occupiedPotties.Add(spotData.pottyOccupied); }
        }

        availablePottiesCount = occupiedPotties.Count(p => p == false);
        if (occupiedPotties.Any(p => p == true))
        {
            if (occupiedPotties.Distinct().Count() == 1) { allOccupied = true; }
            else { allOccupied = false; }
        }
        else
        {
            allOccupied = false;
        }

        occupiedPotties.Clear();
    }
}
