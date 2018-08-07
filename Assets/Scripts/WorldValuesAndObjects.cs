using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldValuesAndObjects : MonoBehaviour {

    public static WorldValuesAndObjects instance;
    public static GameObject[] availablePotties;
    public float amountOfMoney;

    private void Start()
    {
        amountOfMoney = 950.42f;

        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
        }
        instance = this;
    }
}
