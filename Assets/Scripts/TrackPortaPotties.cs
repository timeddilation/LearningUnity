﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TrackPortaPotties : MonoBehaviour {

    public Guid uniqueSim;

    [Header("Sim Configurables")]
    public int bladderSize = 100;    
    public float maxGrossOutLevel = 3f;
    public float disgustingness = 0.5f;
    [Header("Sim In-Game Stats")]
    public int hasToPee = 0;
    public bool isQueued = false;
    public bool desiresPortaPotty = false;
    public bool isConsiderate = true;
    public bool isPermittedByQueue = false;
    [Header("Text Bubbles")]
    public Image grossText;

    private readonly float sightRange = 8;
    private NavMeshAgent agent;
    private GameObject currentQueuePoint = null;
    private Vector3 startPosition;
    private Vector3 queueWaitPosition;
    private float timeToHideGrossText = 0f;
    private readonly float lengthToShowGrossMessage = 1f;
    private bool showingGrossText = false;

    void Start()
    {
        uniqueSim = Guid.NewGuid();
        gameObject.SetActive(true);
        bladderSize = UnityEngine.Random.Range(80, 120);
        disgustingness = UnityEngine.Random.Range(0f, 0.1f);
        startPosition = gameObject.transform.position;
        agent = gameObject.GetComponent<NavMeshAgent>();
        grossText.gameObject.SetActive(false);
        maxGrossOutLevel = UnityEngine.Random.Range(7f, 10f);

        InvokeRepeating("CheckIfNeedToUsePotty", 0f, 0.25f);
    }

    private void Update()
    {
        //move somewhere else after using potty
        if (hasToPee < 5) { agent.SetDestination(startPosition); }

        //increment counter for when sim needs to use potty
        var randy = UnityEngine.Random.Range(0, 3);
        if (randy == 1)
        {
            hasToPee++;
        }

        //handle gross text when present
        if (grossText.gameObject.activeSelf && !showingGrossText)
        {
            showingGrossText = true;
            timeToHideGrossText = Time.time + lengthToShowGrossMessage;
        }
        else if (grossText.gameObject.activeSelf && showingGrossText)
        {
            if (Time.time > timeToHideGrossText)
            {
                showingGrossText = false;
                grossText.gameObject.SetActive(false);
            }
        }

    }

    private void CheckIfNeedToUsePotty()
    {
        //if need to pee, find the closest potty and go to it
        if (hasToPee >= bladderSize)
        {
            desiresPortaPotty = true;

            if (!isQueued)
            {
                FindPotties();
            }
            //handle queing
            else
            {
                //set static wait position while in queue
                if (queueWaitPosition == new Vector3(0, 0, 0))
                {
                    queueWaitPosition = currentQueuePoint.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), 0f, UnityEngine.Random.Range(-2f, 2f));
                }
                else
                {
                    agent.SetDestination(queueWaitPosition);
                }              
                //check to see if at front of queue, and if so, go to a potty
                QueueUp checkQueue = currentQueuePoint.GetComponent<QueueUp>();
                int queueIndexToSearch = checkQueue.availablePottiesCount;
                if (checkQueue.availablePottiesCount > checkQueue.queuedSims.Count())
                {
                    queueIndexToSearch = checkQueue.queuedSims.Count();
                }
                if (!checkQueue.allOccupied && checkQueue.queuedSims.GetRange(0, queueIndexToSearch).Contains(uniqueSim))
                {
                    queueWaitPosition = new Vector3(0, 0, 0);
                    isPermittedByQueue = true;
                    FindPotties();
                }
            }
        }
    }

    private void FindPotties()
    {
        GameObject closestPotty = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        if (WorldValuesAndObjects.availablePotties.Length > 0)
        {
            foreach (GameObject potty in WorldValuesAndObjects.availablePotties)
            {
                SomeoneEntered checkOccupancy = potty.gameObject.GetComponent<SomeoneEntered>();
                Vector3 diff = potty.transform.position - position;
                float curDistance = diff.sqrMagnitude;

                //float dis = Vector3.Distance(position, potty.transform.position);

                bool willConsiderPortaPotty = true;
                if (checkOccupancy.grossedOutSims.Contains(uniqueSim) || checkOccupancy.outOfService)
                {
                    willConsiderPortaPotty = false;
                }                

                if (willConsiderPortaPotty)
                {
                    QueueUp checkQueue = checkOccupancy.spotData.queuePoint.GetComponent<QueueUp>();
                    //when close enough to inspect, do:
                    if (curDistance < sightRange)
                    {
                        bool isConsideringOthersInQueue = false;
                        if (checkQueue.queuedSims.Count() > 0 && isConsiderate && !isPermittedByQueue) { isConsideringOthersInQueue = true; }
                        if (checkQueue.allOccupied || isConsideringOthersInQueue)
                        {
                            JoinQueue(checkQueue);
                        }
                        if (checkOccupancy.isOccupied)
                        {
                            curDistance = Mathf.Infinity;
                        }
                    }
                    if (curDistance < distance)
                    {
                        closestPotty = potty;
                        distance = curDistance;
                    }
                }
            }

            if (closestPotty != null) { agent.SetDestination(closestPotty.transform.position); }
        }
    }

    public void JoinQueue(QueueUp queueToJoin)
    {
        if (!queueToJoin.queuedSims.Contains(uniqueSim)) { queueToJoin.queuedSims.Add(uniqueSim); }
        isQueued = true;
        currentQueuePoint = queueToJoin.gameObject;
    }
}
