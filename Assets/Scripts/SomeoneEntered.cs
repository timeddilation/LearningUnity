using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SomeoneEntered : MonoBehaviour {

    [Header("Potty Stats")]
    public float grossOutLevel = 0;
    public float wasteMatterVolume = 0;
    public List<Guid> grossedOutSims = new List<Guid>();
    public Image portaGrossStat;
    public Image portaWasteMatterStat;    
    [Header("Meta data")]
    public int facingX;
    public int facingZ;
    public bool isOccupied = false;
    public bool outOfService = false;
    private readonly float maxGrossness = 10f;
    private readonly float maxWasteMatterVolume = 10f;
    [Header("In-Game Stored Data")]
    public GameObject storedSim;
    public portaSpotData spotData;
    public QueueUp queueData;
    public GameObject myPortaLocation;
    private int timeInPotty = 0;
    private int timeSimSpendsInPotty;
    [Header("Door Mechanics")]
    public float doorOpenSpeed = 10f;
    public float doorOpenAngle = 75f;
    private int openBufferTimeBeforeEntry = 20;
    private int openBufferTimeElapsed = 0;
    private bool someoneOpeningDoor = false;
    private bool someoneClosingDoor = false;
    private GameObject pottyDoor;
    private Vector3 pottyDoorOpenRotation;
    private Vector3 pottyDoorCloseRotation;

    private void Start()
    {
        spotData = myPortaLocation.GetComponent<portaSpotData>();
        queueData = spotData.queuePoint.GetComponent<QueueUp>();

        pottyDoor = transform.parent.Find("Entrance/Door").gameObject;
        pottyDoorCloseRotation = pottyDoor.transform.rotation.eulerAngles;
        pottyDoorOpenRotation = pottyDoorCloseRotation + new Vector3(0f, doorOpenAngle, 0f);
    }

    private void Update()
    {
        if (isOccupied) { HandleSomeoneUsingRoom(); }
        if (wasteMatterVolume >= maxWasteMatterVolume) { outOfService = true; }
        if (someoneOpeningDoor)
        {
            MoveDoor(pottyDoorOpenRotation);
            if (Mathf.Abs(pottyDoor.transform.rotation.eulerAngles.y - pottyDoorOpenRotation.y) < 1)
            {
                someoneOpeningDoor = false;
                someoneClosingDoor = true;
            }
        }
        if (someoneClosingDoor)
        {
            MoveDoor(pottyDoorCloseRotation);
            if (Mathf.Abs(pottyDoor.transform.rotation.eulerAngles.y - pottyDoorCloseRotation.y) < 1)
            {
                someoneClosingDoor = false;
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sim")) { SomeoneOpensDoor(other); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sim")) { SomeoneOpensDoor(other); }
    }

    private void SomeoneOpensDoor(Collider sim)
    {
        TrackPortaPotties someoneEntering = sim.gameObject.GetComponent<TrackPortaPotties>();
        bool canAllowSimToUse = false;

        if (someoneEntering.isConsiderate && queueData.queuedSims.Count() > 0 && !someoneEntering.isPermittedByQueue)
        {
            someoneEntering.JoinQueue(queueData);
        }
        else if (!isOccupied
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
                someoneOpeningDoor = true;
                someoneClosingDoor = false;
                MoveDoor(pottyDoorOpenRotation);

                //buffer this to wait for some of the door to open
                if (openBufferTimeElapsed < openBufferTimeBeforeEntry) { ++openBufferTimeElapsed; }
                else
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
                    openBufferTimeElapsed = 0;
                }

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
        if (timeSimSpendsInPotty - timeInPotty < 20)
        {
            someoneOpeningDoor = true;
            someoneClosingDoor = false;
        }

        if (timeInPotty <= timeSimSpendsInPotty)
        {
            timeInPotty++;
        }
        else
        {
            TrackPortaPotties simTraits = storedSim.gameObject.GetComponent<TrackPortaPotties>();
            isOccupied = false;
            spotData.pottyOccupied = false;
            //calculate gross level to be added to potty
            grossOutLevel += ((float)simTraits.bladderSize * 0.005f) + simTraits.disgustingness;
            float grossnessMeter = grossOutLevel / maxGrossness;
            portaGrossStat.fillAmount = grossnessMeter;
            //calculate waste matter to be added to potty
            wasteMatterVolume += (float)simTraits.bladderSize * 0.005f;
            float wasteMatterMeter = wasteMatterVolume / maxWasteMatterVolume;
            portaWasteMatterStat.fillAmount = wasteMatterMeter;

            float simHeight = storedSim.transform.position.y - transform.position.y;
            Vector3 position = transform.position;

            if (facingX != 0) { position = position + new Vector3(facingX, simHeight, 0); }
            else { position = position + new Vector3(0, simHeight, facingZ); }

            storedSim.transform.position = position;
            simTraits.isPermittedByQueue = false;
            storedSim.SetActive(true);
            storedSim = null;
            timeInPotty = 0;
        }
    }

    private void MoveDoor(Vector3 endPosition)
    {
        Vector3 smoothOpen = Vector3.Lerp(pottyDoor.transform.rotation.eulerAngles, endPosition, Time.deltaTime * doorOpenSpeed);
        pottyDoor.transform.rotation = Quaternion.Euler(0f, smoothOpen.y, 0f);
    }
}
