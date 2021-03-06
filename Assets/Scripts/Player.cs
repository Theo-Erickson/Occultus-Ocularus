﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float maxWalkSpeed = 1;
    public float walkForce = 20;
    public float jumpForce = 20;

    private Rigidbody2D body;

    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        InputHandler();

    }

    private bool jump = false;
    // For determining whether the player is touching the ground
    private ContactPoint2D[] contactPoints = new ContactPoint2D[5];

    //function to handle button or mouse events to avoid cluttering update
    void InputHandler() {
        //Reset position
        if (Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = Vector3.zero;
        }

        //"Jump"
        if (Input.GetButtonDown("Jump")) {
            //if you can move freely, switch camera modes
            if (antiGrav) {
                CameraScript cScript = Camera.main.GetComponent<CameraScript>();
                if (cScript.mode == CameraScript.CameraMode.Fixed) {
                    cScript.mode = CameraScript.CameraMode.FollowPlayer;
                } else if (cScript.mode == CameraScript.CameraMode.FollowPlayer) {
                    cScript.mode = CameraScript.CameraMode.Fixed;
                    cScript.SetDestination(cScript.pastRoomCameraPosition, 2.0f);
                }
                //if you can't then just "Jump", its placeholder
                // Jump, apply force in FixedUpdate, recommended by Unity docs.
            } else {
                // Only jump if touching the ground
                int count = GetComponent<Collider2D>().GetContacts(contactPoints);
                for(int i = 0; i < count; i ++) {
                    if(Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.5)
                        jump = true;
                }
            }
        }

        //shift between flying and grounded modes
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            if (antiGrav) {
                this.GetComponent<Renderer>().material.color = Color.grey;
                antiGrav = false;
                body.gravityScale = 1;
            } else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
        }
    }

    void FixedUpdate() {
        if (canMove) {

            //if you are allowed free flight
            if (antiGrav) {
                this.transform.position = this.transform.position + Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * maxWalkSpeed;
                this.transform.position = this.transform.position + Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * maxWalkSpeed;
            }
            // When u walkin
            else {
                float h = Input.GetAxis("Horizontal");
                if(h * body.velocity.x < maxWalkSpeed)
                    body.AddForce(Vector2.right * h * walkForce);
                if (Mathf.Abs(body.velocity.x) > maxWalkSpeed)
                    body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxWalkSpeed, body.velocity.y);

                if(jump) {
                    body.AddForce(jumpForce * Vector3.up);
                    jump = false;
                }
            }
        }
    }
}
