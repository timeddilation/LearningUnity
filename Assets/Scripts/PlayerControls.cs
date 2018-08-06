using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            buildManager.pottyToBuild = null;
        }
    }
}
