using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour {

    public Text debugMenu1;

    private int numberOfSims;
    private int numberOfSimsDesiringPotty;
    public int numberOfPotties;
    private int numberOfOccupiedPotties;

    private void Start()
    {
        GameObject[] allSims = GameObject.FindGameObjectsWithTag("Sim");
        numberOfSims = allSims.Length;

        GameObject[] allPotties = GameObject.FindGameObjectsWithTag("Potty");
        numberOfPotties = allPotties.Length;
    }

    void Update ()
    {
        //track sims
        GameObject[] allSims = GameObject.FindGameObjectsWithTag("Sim");
        List<bool> simsDesiringPotty = new List<bool>();
        foreach (GameObject sim in allSims)
        {
            TrackPortaPotties simStats = sim.GetComponent<TrackPortaPotties>();
            simsDesiringPotty.Add(simStats.desiresPortaPotty);
        }
        numberOfSimsDesiringPotty = simsDesiringPotty.Count(s => s == true);

        //track potties
        GameObject[] allPotties = GameObject.FindGameObjectsWithTag("Potty");
        List<bool> occupiedPotties = new List<bool>();
        foreach (GameObject potty in allPotties)
        {
            SomeoneEntered occupancy = potty.GetComponent<SomeoneEntered>();
            occupiedPotties.Add(occupancy.isOccupied);
        }
        numberOfOccupiedPotties = occupiedPotties.Count(p => p == true);

        UpdateDebugMenu();
    }

    private void UpdateDebugMenu()
    {
        string debugText = "";
        debugText += "Sims Desire Potty: " + numberOfSimsDesiringPotty.ToString() + " / " + numberOfSims.ToString();
        debugText += Environment.NewLine + "Potties Occupied: " + numberOfOccupiedPotties.ToString() + " / " + numberOfPotties.ToString();

        debugMenu1.text = debugText;
    }
}
