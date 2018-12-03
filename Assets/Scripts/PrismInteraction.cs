using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismInteraction : MonoBehaviour {

    // Rotates anything tagged 'Prism' when E is pressed
    private void OnTriggerStay2D(Collider2D collision) {

        // Interact key
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If the thing with which you are colliding is a prism, rotate it
            if (collision.tag == "Prism")
            {
                collision.transform.Rotate(new Vector3(0, 0, 45));
            }
        }
    }
}
