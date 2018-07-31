using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TrackPortaPotties : MonoBehaviour {

    public NavMeshAgent agent;
    public int bladderSize = 120;
    public int hasToPee;

    void Start ()
    {
        gameObject.SetActive(true);
        hasToPee = 0;
    }

    private void Update()
    {
        checkIfNeedToUsePotty();
    }

    private void checkIfNeedToUsePotty()
    {
        //if need to pee, find the closest potty and go to it
        if (hasToPee >= bladderSize)
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
                    if (curDistance < 2 && checkOccupancy.isOccupied)
                    {
                        curDistance = Mathf.Infinity;
                    }
                    if (curDistance < distance)
                    {
                        closestPotty = potty;
                        distance = curDistance;
                    }
                }

                agent.SetDestination(closestPotty.transform.position);
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
}
