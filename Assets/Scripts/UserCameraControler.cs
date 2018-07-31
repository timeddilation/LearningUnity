using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraControler : MonoBehaviour {

    public float zoomSpeed = 7f;
    public float dragSpeed = 0.55f;

    private void LateUpdate()
    {
        //Zoom in and out based on scroll whee;
        float cameraTransposeMagnitude = zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
        transform.position = transform.position + new Vector3(0, -cameraTransposeMagnitude, cameraTransposeMagnitude);

        //drag camera around 2D XY plane
        if (Input.GetMouseButton(0))
        {
            float dragX = Input.GetAxis("Mouse X");
            float dragY = Input.GetAxis("Mouse Y");
            float cameraDragMagnitudeX = dragX * dragSpeed;
            float cameraDragMagnitudeY = dragY * dragSpeed;
            transform.position = transform.position + new Vector3(-cameraDragMagnitudeX, 0, -cameraDragMagnitudeY);
        }
    }
}
