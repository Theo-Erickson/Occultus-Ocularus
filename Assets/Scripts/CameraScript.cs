using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraMode { Fixed, FollowPlayer }

    [Header("Camera Mode")]
    [Tooltip("Does the camera stay still or follow the player?")]
    public CameraMode mode;

    [Header("Positioning")]
    [Tooltip("Used so the camera knows where it came from")]
    public Vector3 start;
    [Tooltip("Tells the camera where to go")] public Vector3 destination;
    public Vector3 pastRoomCameraPosition;


    private GameObject player;
    private float timeToReachTarget = 5.0f;
    private float t;


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
            } else {
                player.GetComponent<Player>().canMove = true;
            }

            //Camera is set to follow player
        } else if (mode == CameraMode.FollowPlayer) {
            //So unity only makes it happen once for better performance
            if (this.transform.parent != player.transform) {
                this.transform.parent = player.transform;
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
