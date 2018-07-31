using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    //debug shit
    public Text debugMenu;
    private string playerVelocity;

    public float acceleration;
    public float jumpHeight;
    public Text countText;
    public Text winText;
    public Text spedometerWindow;
    public GameObject respawnPoint;

    private Rigidbody rb;
    private int countOfPickUps;
    //private bool hasWon;
    private Vector3 lastPosition = Vector3.zero;
    private bool playerIsProvidingInput;
    private decimal spedometer;
    private bool canStop;
    private bool isGrounded;

    private void Start()
    {
        debugMenu.text = "";

        rb = GetComponent<Rigidbody>();
        countOfPickUps = 0;
        UpdatePickUpsDisplay();
        winText.text = "";
        //hasWon = false;
        playerIsProvidingInput = false;
        canStop = false;
        isGrounded = false;
    }

    public void Update()
    {
        playerVelocity = rb.velocity.magnitude.ToString();
        UpdateDebugMenu();

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
        spedometerWindow.text = Math.Round((spedometer), 2).ToString();
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Pick up collectable, and add to counter
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            countOfPickUps = ++countOfPickUps;
            UpdatePickUpsDisplay();

            transform.localScale = transform.localScale + new Vector3(0.05f, 0.05f, 0.05f);
            transform.position = transform.position + new Vector3(0, 0.025f, 0);
        }
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

    void UpdatePickUpsDisplay()
    {
        countText.text = "Collectables: " + countOfPickUps.ToString();
        if (countOfPickUps >= 12)
        {
            winText.text = "YOU WIN!";
            //hasWon = true;
        }
    }

    void UpdateDebugMenu()
    {
        debugMenu.text = playerVelocity;
    }
}
