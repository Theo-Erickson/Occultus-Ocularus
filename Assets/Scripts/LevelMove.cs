using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMove : MonoBehaviour {
    public Camera cam;
    [Header("Positioning")]
    [Tooltip("Where the camera originally was")]public Vector3 origin;
    [Tooltip("Where it should go to")]public Vector3 destination;

    public float moveTime = 2.0f;

	// Use this for initialization
	void Start () {
        if(cam == null) { cam = Camera.main; }

	}
	
    void Update() {
        GameObject other = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {

        ///This is to check if you are on same y height as this object    
            //serves to allow checking in you approached from left or right
            if (Mathf.Abs(other.transform.position.x - transform.position.x) < this.GetComponent<BoxCollider>().size.x / 2) {
                //you have entered from the top
                if (other.transform.position.y > this.transform.position.y) {
                    Camera.main.GetComponent<CameraScript>().SetDestination(destination, moveTime);
                } else { //you entered from the bottom
                    Camera.main.GetComponent<CameraScript>().SetDestination(origin, moveTime);
                }
            } else if (Mathf.Abs(other.transform.position.y - transform.position.y) > this.GetComponent<BoxCollider>().size.y / 2) {
                //enter from the right
                if (other.transform.position.x > this.transform.position.x) {
                    Camera.main.GetComponent<CameraScript>().SetDestination(origin, moveTime);
                } else { //you entered from the left 
                    Camera.main.GetComponent<CameraScript>().SetDestination(destination, moveTime);
                }
            }
        }
    }
}
