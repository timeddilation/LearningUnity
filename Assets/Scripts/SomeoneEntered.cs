﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SomeoneEntered : MonoBehaviour {

    public GameObject storedSim;
    public Image portaGrossStat;
    public Text debugMenu;
    public int facingX;
    public int facingZ;
    public int queueSize;
    public bool isOccupied = false;
    public bool outOfService = false;
    public float grossOutLevel;
    public List<Guid> grossedOutSims = new List<Guid>();
    public portaSpotData spotData;
    public QueueUp queueData;

    private GameObject myPortaLocation = null;

    private int timeInPotty;
    private int timeSimSpendsInPotty;
    private float simHeight;

    private readonly float maxGrossness = 4f;

    private void Start()
    {
        //grossedOutSims.Add(Guid.NewGuid());
        timeInPotty = 0;
        simHeight = 0;
        queueSize = 0;
        grossOutLevel = 0;

        FindMyPortaLocation();
        spotData = myPortaLocation.GetComponent<portaSpotData>();
        spotData.hasPotty = true;
        queueData = spotData.queuePoint.GetComponent<QueueUp>();
    }

    private void Update()
    {
        HandleSomeUsingRoom();
        if (grossOutLevel >= maxGrossness) { outOfService = true; }
        //debugMenu.text = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        TrackPortaPotties someoneEntering = other.gameObject.GetComponent<TrackPortaPotties>();
        bool canAllowSimToUse = false;
        if (other.gameObject.CompareTag("Sim")
            && !isOccupied
            && !outOfService
            && !grossedOutSims.Contains(someoneEntering.uniqueSim)
            && someoneEntering.desiresPortaPotty)
        {
            canAllowSimToUse = true;
        }

        if (canAllowSimToUse)
        {
            if (grossOutLevel <= someoneEntering.maxGrossOutLevel)
            {
                someoneEntering.hasToPee = 0;
                someoneEntering.gameObject.SetActive(false);
                someoneEntering.isQueued = false;
                someoneEntering.desiresPortaPotty = false;
                queueData.queuedSims.RemoveAll(q => q == someoneEntering.uniqueSim);
                storedSim = other.gameObject;
                isOccupied = true;
                spotData.pottyOccupied = true;
                simHeight = other.transform.position.y - transform.position.y;
                timeSimSpendsInPotty = someoneEntering.bladderSize;               
            }
            else
            {
                grossedOutSims.Add(someoneEntering.uniqueSim);
            }
        }
    }

    private void HandleSomeUsingRoom()
    {
        if (isOccupied)
        {
            if (timeInPotty <= timeSimSpendsInPotty)
            {
                timeInPotty = ++timeInPotty;
            }
            else
            {
                isOccupied = false;
                spotData.pottyOccupied = false;
                grossOutLevel += UnityEngine.Random.Range(0f, 1f);
                float grossnessMeter = grossOutLevel / maxGrossness;
                portaGrossStat.fillAmount = grossnessMeter;

                Vector3 position = transform.position;
                if (facingX != 0)
                {
                    position = position + new Vector3(facingX, simHeight, 0);
                }
                else
                {
                    position = position + new Vector3(0, simHeight, facingZ);
                }

                storedSim.transform.position = position;
                storedSim.SetActive(true);
                storedSim = null;
                timeInPotty = 0;
                simHeight = 0;
            }
        }
    }

    private void FindMyPortaLocation()
    {
        GameObject[] pottyLocations = GameObject.FindGameObjectsWithTag("SpawnPotty");
        GameObject closestPottyLocation = null;
        float distance = Mathf.Infinity;

        foreach (GameObject pottyLocation in pottyLocations)
        {
            Vector3 diff = pottyLocation.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closestPottyLocation = pottyLocation;
                distance = curDistance;
            }
        }

        myPortaLocation = closestPottyLocation;
    }
}
