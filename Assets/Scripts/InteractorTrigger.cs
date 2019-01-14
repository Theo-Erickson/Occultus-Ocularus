using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * This interactor causes an interaction when something enters or exits its trigger.
 */
public class InteractorTrigger : MonoBehaviour
{
    [Tooltip("Objects with this tag will cause interactions. If tag is empty, all objects will cause interactions.")] 
    public string objectTag = "Player";
    [Tooltip("Whether or not only objects residing in the same layer as this object will cause interactions.")]
    public bool sameLayerOnly = true;
    [Tooltip("This is the object that is interacted with when something enters this trigger. This value may be None.")]
    public GameObject enterInteractable = null;
    [Tooltip("This is the ID of interaction that is triggered when something enters this trigger.")]
    public int enterID = 0;
    [Tooltip("This is the object that is interacted with when something leaves this trigger. This value may be None.")]
    public GameObject exitInteractable = null;
    [Tooltip("This is the ID of interaction that is triggered when something leaves this trigger.")]
    public int exitID = 0;
    [Tooltip("This is the object that is sent to the interactables as sender when an interaction occurs."
        + "If this is None, this trigger itself will be sent.")]
    public GameObject sender = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If no tag is specified or the object's tag matches the specified tag.
        if(String.IsNullOrEmpty(tag) || other.tag == objectTag) {
            // If sameLayerOnly is false or the object is in the same layer.
            if(!sameLayerOnly || other.gameObject.layer == gameObject.layer) {
                if(enterInteractable == null)
                    return;
                bool interacted = false;
                Interaction[] interactions = enterInteractable.GetComponents<Interaction>();
                foreach(Interaction interaction in interactions)
                {
                    if(interaction != null && interaction.ID == enterID) {
                        interaction.Interact(sender != null ? sender : this.gameObject);
                        interacted = true;
                    }
                }
                if(!interacted)
                    Debug.Log("WARNING! Trying to interact with an object with no interaction!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If no tag is specified or the object's tag matches the specified tag.
        if(String.IsNullOrEmpty(tag) || other.tag == objectTag) {
            // If sameLayerOnly is false or the object is in the same layer.
            if(!sameLayerOnly || other.gameObject.layer == gameObject.layer) {
                if(exitInteractable == null)
                    return;
                bool interacted = false;
                Interaction[] interactions = exitInteractable.GetComponents<Interaction>();
                foreach(Interaction interaction in interactions)
                {
                    if(interaction != null && interaction.ID == exitID) {
                        interaction.Interact(sender != null ? sender : this.gameObject);
                        interacted = true;
                    }
                }
                if(!interacted)
                    Debug.Log("WARNING! Trying to interact with an object with no interaction!");
            }
        }
    }
}
