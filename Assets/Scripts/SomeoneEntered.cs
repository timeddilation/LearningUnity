using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SomeoneEntered : MonoBehaviour {

    public GameObject storedSim;
    public Text debugMenu;
    public int facingX;
    public int facingZ;
    public int queueSize;
    public bool isOccupied = false;

    private int timeInPotty;
    private int timeSimSpendsInPotty;
    private float simHeight;

    private void Start()
    {
        timeInPotty = 0;
        simHeight = 0;
        queueSize = 0;
    }

    private void Update()
    {
        handleSomeUsingRoom();
        //debugMenu.text = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sim") && !isOccupied)
        {
            TrackPortaPotties someoneEntering = other.gameObject.GetComponent<TrackPortaPotties>();
            if (someoneEntering != null)
            {
                someoneEntering.hasToPee = 0;
                someoneEntering.gameObject.SetActive(false);
            }

            DebugTrackPortaPotties someoneEnterDebug = other.gameObject.GetComponent<DebugTrackPortaPotties>();
            if (someoneEnterDebug != null)
            {
                someoneEnterDebug.hasToPee = 0;
                someoneEnterDebug.gameObject.SetActive(false);
            }          

            storedSim = other.gameObject;
            isOccupied = true ;
            simHeight = other.transform.position.y - transform.position.y;
            timeSimSpendsInPotty = someoneEntering.bladderSize;
        }
    }

    private void handleSomeUsingRoom()
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
}
