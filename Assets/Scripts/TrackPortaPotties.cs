using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TrackPortaPotties : MonoBehaviour {

    public Guid uniqueSim;

    public NavMeshAgent agent;
    public int bladderSize = 120;
    public int hasToPee;
    public float maxGrossOutLevel;
    public bool isQueued = false;
    public bool desiresPortaPotty = false;

    private readonly float sightRange = 8;

    private GameObject currentQueuePoint = null;
    private Vector3 startPosition;

    void Start()
    {
        uniqueSim = Guid.NewGuid();
        gameObject.SetActive(true);
        hasToPee = 0;
        startPosition = gameObject.transform.position;
        maxGrossOutLevel = 10f;
    }

    private void Update()
    {
        CheckIfNeedToUsePotty();
        if (hasToPee < 5) { agent.SetDestination(startPosition); }
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
                agent.SetDestination(currentQueuePoint.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)));

                QueueUp checkQueue = currentQueuePoint.GetComponent<QueueUp>();
                int queueIndexToSearch = checkQueue.availablePottiesCount;
                if (checkQueue.availablePottiesCount > checkQueue.queuedSims.Count())
                {
                    queueIndexToSearch = checkQueue.queuedSims.Count();
                }
                if (!checkQueue.allOccupied && checkQueue.queuedSims.GetRange(0, queueIndexToSearch).Contains(uniqueSim))
                {
                    FindPotties();
                }
            }
        }
        else
        {
            var randy = UnityEngine.Random.Range(0, 3);
            if (randy == 1)
            {
                hasToPee = ++hasToPee;
            }

        }
    }

    private void FindPotties()
    {
        GameObject[] availablePotties = GameObject.FindGameObjectsWithTag("Potty");
        GameObject closestPotty = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        if (availablePotties.Length > 0)
        {
            foreach (GameObject potty in availablePotties)
            {
                SomeoneEntered checkOccupancy = potty.gameObject.GetComponent<SomeoneEntered>();
                Vector3 diff = potty.transform.position - position;
                float curDistance = diff.sqrMagnitude;

                if (!checkOccupancy.grossedOutSims.Contains(uniqueSim))
                {
                    QueueUp checkQueue = checkOccupancy.spotData.queuePoint.GetComponent<QueueUp>();

                    if (curDistance < sightRange)
                    {                        
                        if (!checkQueue.queuedSims.Contains(uniqueSim) && (checkQueue.allOccupied || checkQueue.queuedSims.Count > 0))
                        {
                            checkQueue.queuedSims.Add(uniqueSim);
                            isQueued = true;
                            currentQueuePoint = checkOccupancy.spotData.queuePoint;
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

            //bug here hwen only one available potty: object ref error
            agent.SetDestination(closestPotty.transform.position);
        }
    }
}
