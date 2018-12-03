using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismInteraction : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Prism")
        {
            collision.transform.Rotate(new Vector3(0, 0, 45));
        }
    }
}
