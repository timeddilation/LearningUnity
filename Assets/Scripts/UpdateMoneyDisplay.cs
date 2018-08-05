using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoneyDisplay : MonoBehaviour {

    public static UpdateMoneyDisplay instance;

    public Text moneyDisplay;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        moneyDisplay.text = "$" + WorldValuesAndObjects.instance.amountOfMoney.ToString("0.00");
    }
}
