using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldValuesAndObjects : MonoBehaviour {

    public static WorldValuesAndObjects instance;
    public static GameObject[] availablePotties;
    public decimal amountOfMoney;

    private void Start()
    {
        amountOfMoney = (decimal)950.42;

        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
        }
        instance = this;
    }
}
