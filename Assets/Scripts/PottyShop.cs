using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PottyShop : MonoBehaviour {

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseStandardPotty()
    {
        buildManager.SetPottyToBuild(buildManager.standardPottyPrefab);
    }
}
