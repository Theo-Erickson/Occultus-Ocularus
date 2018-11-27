using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float moveSpeed = 1;
    public float jumpForce = 20;

    // Use this for initialization
    void Start() {

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
                this.GetComponent<Rigidbody2D>().gravityScale = 1;
            } else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                this.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    void FixedUpdate() {
        if (canMove) {
            this.transform.position = this.transform.position + Vector3.right * Input.GetAxis("Horizontal") * 0.1f * moveSpeed;

            if(jump) {
                this.GetComponent<Rigidbody2D>().AddForce(jumpForce * Vector3.up);
                jump = false;
            }

            //if you are allowed free flight
            if (antiGrav) {
                this.transform.position = this.transform.position + Vector3.up * Input.GetAxis("Vertical") * 0.1f * moveSpeed;
            }
        }
    }
}
