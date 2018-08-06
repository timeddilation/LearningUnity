using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class portaSpotData : MonoBehaviour {

    [Header("Hovor Controls")]
    public Material hoverMaterial;
    public Material hovorMaterialCantBuild;
    private Material startingMatarial;
    private Renderer rend;

    [Header("Meta Data")]
    public int facingX = -1;
    public int facingZ = 0;
    public bool hasPotty = false;
    public bool pottyOccupied = false;
    public GameObject queuePoint = null;

    BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startingMatarial = rend.material;

        buildManager = BuildManager.instance;
    }

    private void OnMouseDown()
    {
        //can't click if mouse is over UI element
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        //can't build where there is already a potty
        if (hasPotty)
        {
            Debug.Log("Already a potty there!");
            return;
        }
        //can't build if there is nothing selected to build
        else if (buildManager.GetPottyToBuild() == null)
        {
            return;
        }
        //not enough potties in inventory
        else if (buildManager.pottiesSpawned >= buildManager.pottiesToSpawn)
        {
            Debug.Log("Not enough available potties to place!");
            return;
        }
        //not enough money
        else if (WorldValuesAndObjects.instance.amountOfMoney < buildManager.standardPottyCosts.cost)
        {
            Debug.Log("Not enough money to purchase!");
        }
        //build potty
        else
        {
            BuildPotty();
            InformGameManagerOfPotties();
        }       
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (!buildManager.hasEnoughMoneyToPurchase) { rend.material = hovorMaterialCantBuild; }
        else if (buildManager.GetPottyToBuild() != null) { rend.material = hoverMaterial; }
            
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
        GameObject pottyToBuild = buildManager.GetPottyToBuild();
        float spawnRotation = GetRotationOfPotty(facingX, facingZ);
        //instantiate potty and get entrance script to set values
        GameObject potty = Instantiate(pottyToBuild, gameObject.transform.position, Quaternion.Euler(0, spawnRotation, 0));
        PottyData pottyCosts = potty.GetComponent<PottyData>();
        GameObject pottyEntrance = potty.transform.Find("EntranceTrigger").gameObject;
        SomeoneEntered pottySetup = pottyEntrance.GetComponent<SomeoneEntered>();
        //set where and rotation on potty
        potty.SetActive(true);
        potty.transform.parent = transform;
        pottySetup.facingX = facingX;
        pottySetup.facingZ = facingZ;
        pottySetup.myPortaLocation = gameObject;
        hasPotty = true;
        //update build manager with number of potties spawned, and rebuild navMesh
        buildManager.pottiesSpawned++;
        buildManager.surface.BuildNavMesh();
        WorldValuesAndObjects.instance.amountOfMoney -= pottyCosts.cost;
        //Update if you have enough money to build curently selected potty
        if (pottyCosts.cost > WorldValuesAndObjects.instance.amountOfMoney)
        {
            buildManager.hasEnoughMoneyToPurchase = false;
        }
        else
        {
            buildManager.hasEnoughMoneyToPurchase = true;
        }
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
