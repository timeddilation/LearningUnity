using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SomeoneEntered : MonoBehaviour {

    public GameObject storedSim;
    public Image portaGrossStat;
    public int facingX;
    public int facingZ;
    public bool isOccupied = false;
    public bool outOfService = false;
    //TO DO: separate logic of grossness and fullness for out of serive logic
    //public float maxWasteVolume = 12;
    public float grossOutLevel;
    public List<Guid> grossedOutSims = new List<Guid>();
    public portaSpotData spotData;
    public QueueUp queueData;

    private GameObject myPortaLocation = null;
    private int timeInPotty;
    private int timeSimSpendsInPotty;

    private readonly float maxGrossness = 4f;

    private void Start()
    {
        timeInPotty = 0;
        grossOutLevel = 0;

        FindMyPortaLocation();
        spotData = myPortaLocation.GetComponent<portaSpotData>();
        queueData = spotData.queuePoint.GetComponent<QueueUp>();
    }

    private void Update()
    {
        HandleSomeoneUsingRoom();
        if (grossOutLevel >= maxGrossness) { outOfService = true; }
    }

    private void OnTriggerEnter(Collider other)
    {
        SomeoneOpensDoor(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SomeoneOpensDoor(other);
    }

    private void SomeoneOpensDoor(Collider sim)
    {
        TrackPortaPotties someoneEntering = sim.gameObject.GetComponent<TrackPortaPotties>();
        bool canAllowSimToUse = false;
        if (sim.gameObject.CompareTag("Sim")
            && !isOccupied
            && !outOfService
            && !grossedOutSims.Contains(someoneEntering.uniqueSim)
            && someoneEntering.desiresPortaPotty)
        {
            canAllowSimToUse = true;
        }

        if (canAllowSimToUse)
        {
            //sim opens door, can evaluate gross level at this point
            if (grossOutLevel <= someoneEntering.maxGrossOutLevel)
            {
                storedSim = sim.gameObject;
                timeSimSpendsInPotty = someoneEntering.bladderSize;
                isOccupied = true;
                spotData.pottyOccupied = true;
                queueData.queuedSims.RemoveAll(q => q == someoneEntering.uniqueSim);
                someoneEntering.hasToPee = 0;
                someoneEntering.gameObject.SetActive(false);
                someoneEntering.isQueued = false;
                someoneEntering.desiresPortaPotty = false;
            }
            else
            {
                grossedOutSims.Add(someoneEntering.uniqueSim);
                someoneEntering.grossText.gameObject.SetActive(true);
            }
        }
    }

    private void HandleSomeoneUsingRoom()
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

                float simHeight = storedSim.transform.position.y - transform.position.y;
                Vector3 position = transform.position;

                if (facingX != 0) { position = position + new Vector3(facingX, simHeight, 0); }
                else { position = position + new Vector3(0, simHeight, facingZ); }

                storedSim.transform.position = position;
                storedSim.SetActive(true);
                storedSim = null;
                timeInPotty = 0;
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
