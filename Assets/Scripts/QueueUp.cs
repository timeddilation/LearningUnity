using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueUp : MonoBehaviour {

    public GameObject[] portaSpots;

    private void Start()
    {
        foreach (GameObject portaSpot in portaSpots)
        {
            portaSpotData spotData = portaSpot.gameObject.GetComponent<portaSpotData>();
        }
    }
}
