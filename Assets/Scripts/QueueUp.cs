using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueUp : MonoBehaviour {

    public List<GameObject> portaSpots;
    public bool allOccupied;

    private void Update()
    {
        List<bool> occupiedPotties = new List<bool>();

        foreach (GameObject portaSpot in portaSpots)
        {
            portaSpotData spotData = portaSpot.gameObject.GetComponent<portaSpotData>();   
            if (spotData.hasPotty) { occupiedPotties.Add(spotData.pottyOccupied); }
        }

        if (occupiedPotties.Any(p => p == true))
        {
            if (occupiedPotties.Distinct().Count() == 1) { allOccupied = true; }
        }
        else
        {
            allOccupied = false;
        }

        occupiedPotties.Clear();
    }
}
