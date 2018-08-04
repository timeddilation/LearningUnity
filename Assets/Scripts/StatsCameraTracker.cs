using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCameraTracker : MonoBehaviour {

    public Camera cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update ()
    {
        transform.LookAt(2 * transform.position - cam.transform.position);
        //this version of look at has images backwards
        //transform.LookAt(cam.transform);
    }
}
