using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraMode { Fixed, FollowPlayer, FollowPlayerRadius }

    [Header("Camera Mode")]
    [Tooltip("Does the camera stay still, follow the player, or follow within a radius?")]
    public CameraMode mode;

    [Header("Positioning")]
    [Tooltip("Used so the camera knows where it came from")]
    public Vector3 start;
    [Tooltip("Tells the camera where to go")] public Vector3 destination;
    public Vector3 pastRoomCameraPosition;

    [Tooltip("How far the player can move from the view center before the camera will follow it (Applies only to the FollowPlayerRadius camera mode)")]
    public float followRadius = 3;

    private GameObject player;
    private float timeToReachTarget = 5.0f;
    private float t;
    // these only apply to the FollowPlayerRadius game mode:
    private Vector2 screenCenterVector; // Stores the coordinates representing the center of the camera view in world space (2d).
    private Vector2 screenToPlayerVector; // Stores the vector between the center of the camera view in world space to the player's position (2d).


    void Start() {
        pastRoomCameraPosition = start;
        player = GameObject.Find("Player");
        start = destination = transform.position;
    }

    void Update() {
        //Camera is a set a point and does not travel with player
        if (mode == CameraMode.Fixed) {
            if (this.transform.parent == player.transform) {
                this.transform.parent = null;
            }
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(start, destination, t);

            //Disable the player movement when the camera is shifting
            if (Vector3.Distance(transform.position, destination) > 1 && Vector3.Distance(transform.position, start) > 1) {
                GameObject.Find("Player").GetComponent<Player>().canMove = false;
                //Stop the player from moving when you are transitioning with the camera 
                GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            } else {
                player.GetComponent<Player>().canMove = true;
                //Give player the contraints on all rotations + Z axis position
                GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            }

            //Camera is set to follow player
        } else if (mode == CameraMode.FollowPlayer) {
            //So unity only makes it happen once for better performance
            if (this.transform.parent != player.transform) {
                this.transform.parent = player.transform;
            }
        } else if (mode == CameraMode.FollowPlayerRadius) {
            //get the center of the screen (mainCamera) in the unity world units
            screenCenterVector = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2), (Screen.height / 2)));
            screenToPlayerVector = (Vector2)player.transform.position - screenCenterVector; //subtracts the center of the screen coordinates (2d) from the player's coordinates;
            if (screenToPlayerVector.magnitude > followRadius) // if the player is too far from the center of the view:
            {
                //move the camera such that the player is within the radius focusAreaSize of the view center again:
                transform.Translate((screenToPlayerVector - screenToPlayerVector.normalized * followRadius) / 2, Space.World);
            }
        }
    }

    public void resetPosition(Vector3 destination, float time) {
        t = 0;
        start = transform.position;
        timeToReachTarget = time;
        destination = start;
    }

    //Use to tell the camera where to move to in fixed mode
    public void SetDestination(Vector3 destination, float time) {
        t = 0;
        start = transform.position;
        timeToReachTarget = time;
        this.destination = destination;
    }
}
