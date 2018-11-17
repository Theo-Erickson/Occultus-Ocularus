using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float moveSpeed = 1;

    // Use this for initialization
    void Start() {

    }

    void Update() {
        InputHandler();

    }


    //function to handle button or mouse events to avoid cluttering update
    void InputHandler() {
        //Reset position
        if (Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = Vector3.zero;
        }

        //"Jump"
        if (Input.GetKeyDown(KeyCode.Space)) {
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
            } else {
                this.transform.position = this.transform.position + Vector3.up * 2.5f;
            }
        }

        //shift between flying and grounded modes
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            if (antiGrav) {
                this.GetComponent<Renderer>().material.color = Color.grey;
                antiGrav = false;
                this.GetComponent<Rigidbody>().useGravity = true;
            } else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                this.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    void FixedUpdate() {
        if (canMove) {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                this.transform.position = this.transform.position + Vector3.left * 0.1f * moveSpeed;
            } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                this.transform.position = this.transform.position + Vector3.right * 0.1f * moveSpeed;
            }

            //if you are allowed free flight
            if (antiGrav) {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    this.transform.position = this.transform.position + Vector3.up * 0.1f * moveSpeed;
                } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    this.transform.position = this.transform.position + Vector3.down * 0.1f * moveSpeed;
                }
            }
        }
    }
}
