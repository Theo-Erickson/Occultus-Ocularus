using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * This interactor causes an interaction when something enters or exits its trigger and the player presses 'Interact'.
 */
public class InteractorButton : MonoBehaviour
{
    [Tooltip("Objects with this tag will cause interactions. If tag is empty, all objects will cause interactions.")] 
    public string objectTag = "Player";
    [Tooltip("Whether or not only objects residing in the same layer as this object will cause interactions.")]
    public bool sameLayerOnly = true;
    [Tooltip("This is the object that is interacted with when something is in this trigger and the 'Interact' key is pressed."
     + " This value may be None.")]
    public GameObject interactable = null;
    [Tooltip("This is the ID of interaction that is triggered when something is in this trigger and the 'Use' key is pressed.")]
    public int ID = 0;
    [Tooltip("This is the object that is sent to the interactables as sender when an interaction occurs."
        + "If this is None, this trigger itself will be sent.")]
    public GameObject sender = null;

    public List<GameObject> inTrigger = new List<GameObject>();
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")) {
            foreach(GameObject go in inTrigger) {
                // If no tag is specified or the object's tag matches the specified tag.
                if(String.IsNullOrEmpty(objectTag) || go.tag == objectTag) {
                    // If sameLayerOnly is false or the object is in the same layer.
                    if(!sameLayerOnly || go.gameObject.layer == gameObject.layer) {
                        triggered = true;
                        break;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(triggered == true) {
            if(interactable == null)
                return;
            bool interacted = false;
            Interaction[] interactions = interactable.GetComponents<Interaction>();
            foreach(Interaction interaction in interactions)
            {
                if(interaction != null && interaction.ID == ID) {
                    interaction.Interact(sender != null ? sender : this.gameObject);
                    interacted = true;
                }
            }
            if(!interacted)
                Debug.Log("WARNING! Trying to interact with an object with no interaction!");
            triggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't do anything if the other collider is a trigger.
        if(other.isTrigger)
            return;

        inTrigger.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.isTrigger)
            return;
        
        inTrigger.Remove(other.gameObject);
    }
}
