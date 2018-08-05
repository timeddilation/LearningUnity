using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portaSpotData : MonoBehaviour {

    [Header("Hovor Controls")]
    public Material hoverMaterial;
    private Material startingMatarial;
    private Renderer rend;

    [Header("Meta Data")]
    public int facingX = -1;
    public int facingZ = 0;
    public bool hasPotty = false;
    public bool pottyOccupied = false;
    public GameObject queuePoint = null;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startingMatarial = rend.material;
    }

    private void OnMouseDown()
    {
        if (hasPotty)
        {
            Debug.Log("Already a potty there!");
            return;
        }
        else
        {
            if (BuildManager.instance.pottiesSpawned < BuildManager.instance.pottiesToSpawn)
            {
                BuildPotty();
                InformGameManagerOfPotties();
            }
            else
            {
                Debug.Log("Not enough available potties to place!");
            }
        }       
    }

    private void OnMouseEnter()
    {
        rend.material = hoverMaterial;
    }

    private void OnMouseExit()
    {
        rend.material = startingMatarial;
    }

    private float GetRotationOfPotty(int facingX, int facingZ)
    {
        float rotationValue = 0f;
        if (facingX < 0) { rotationValue = 0f; }
        else if (facingX > 0) { rotationValue = 180f; }
        else if (facingZ < 0) { rotationValue = 90f; }
        else if (facingZ > 0) { rotationValue = -90f; }
        return rotationValue;
    }

    private void BuildPotty()
    {
        GameObject pottyToBuild = BuildManager.instance.GetPottyToBuild();
        float spawnRotation = GetRotationOfPotty(facingX, facingZ);
        //instantiate potty and get entrance script to set values
        GameObject potty = Instantiate(pottyToBuild, gameObject.transform.position, Quaternion.Euler(0, spawnRotation, 0));
        GameObject pottyEntrance = potty.transform.Find("EntranceTrigger").gameObject;
        SomeoneEntered pottySetup = pottyEntrance.GetComponent<SomeoneEntered>();
        //set where and rotation on potty
        potty.transform.parent = transform;
        pottySetup.facingX = facingX;
        pottySetup.facingZ = facingZ;
        pottySetup.myPortaLocation = gameObject;
        hasPotty = true;
        //update build manager with number of potties spawned, and rebuild navMesh
        BuildManager.instance.pottiesSpawned++;
        BuildManager.instance.surface.BuildNavMesh();
    }

    private void InformGameManagerOfPotties()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        DebugMenu debugPotties = gameManager.GetComponent<DebugMenu>();

        GameObject[] allPotties = GameObject.FindGameObjectsWithTag("Potty");
        debugPotties.numberOfPotties = allPotties.Length;

        WorldValuesAndObjects.availablePotties = allPotties;
    }
}
