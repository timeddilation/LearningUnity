using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerControl : MonoBehaviour {

    public GameObject respawnPoint;
    public float acceleration;
    public float jumpHeight;

    private Rigidbody rb;
    private Vector3 lastPosition = Vector3.zero;
    private bool playerIsProvidingInput;
    private bool canStop;
    private bool isGrounded;
    private decimal spedometer;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        playerIsProvidingInput = false;
        canStop = false;
        isGrounded = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool jump = Input.GetButtonDown("Jump");
        if (jump && isGrounded)
        {
            Vector3 jumping = new Vector3(0.0f, 1f, 0.0f);
            rb.AddForce(jumping * jumpHeight);

            isGrounded = false;
        }

        if (!playerIsProvidingInput && isGrounded && canStop && spedometer < 1)
        {
            //transform.position = lastPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        playerIsProvidingInput = (moveHorizontal != 0 | moveVertical != 0);

        //Movement is different between on ground and in the air
        if (isGrounded)
        {
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * acceleration);
        }
        else
        {
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * (acceleration * .15f));
        }

        //Put the speed value in
        spedometer = Convert.ToDecimal((transform.position - lastPosition).magnitude * 100);
        //spedometerWindow.text = Math.Round((spedometer), 2).ToString();
        lastPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canStop = true;
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Slope"))
        {
            canStop = false;
            isGrounded = true;
        }

        //Respawn when death occurs
        if (collision.gameObject.CompareTag("Death"))
        {
            transform.position = respawnPoint.transform.position;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") | collision.gameObject.CompareTag("Slope"))
        {
            isGrounded = false;
        }
    }
}
