using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 25, 0) * Time.deltaTime, Space.World);
	}
}
