using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraControler : MonoBehaviour {

    public float zoomSpeed = 7f;
    public float dragSpeed = 0.55f;
    public float rotateSpeed = 3f;

    private float lookX;
    private float lookY;

    private void Start()
    {
        //set the inital pitch and yaw to camer start position
        lookX = transform.rotation.eulerAngles.x;
        lookY = transform.rotation.eulerAngles.y;
    }

    private void LateUpdate()
    {
        //Zoom in and out based on scroll wheel
        float cameraTransposeMagnitude = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        transform.Translate(0, 0, cameraTransposeMagnitude, Space.Self);

        //drag camera around 2D XY plane
        if (Input.GetMouseButton(0))
        {
            float dragX = Input.GetAxis("Mouse X") * dragSpeed;
            float dragY = Input.GetAxis("Mouse Y") * dragSpeed;
            transform.Translate(-dragX, -dragY, 0);
        }
        //rotate camera
        else if (Input.GetMouseButton(1))
        {
            lookY += rotateSpeed * Input.GetAxis("Mouse X");
            lookX -= rotateSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(lookX, lookY, 0f);
        }
    }
}
