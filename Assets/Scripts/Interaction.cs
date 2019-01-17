using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * This component's descendents can be added to objects to give make them interactable.
 * This is the "effect" of an interaction. Effects can be caused by the Interactor classes,
 * e.g., InteractorButton, InteractorTrigger, etc.
 */
[Serializable]
public abstract class Interaction : MonoBehaviour
{
    [Tooltip(
     "This is the game object that will be interacted with. "
     + "If it is set to null, this object itself will be interacted with. "
     + "This is useful if for example, the object that should be interacted with "
     + "is a child of some other object that should handle the interactions.")]
    public GameObject targetObject = null;

    [Tooltip("The ID of this interaction. This should be used if an object has more than one interaction.")]
    public int ID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Called by triggers, for example, a button. Sender is the object, e.g. button, that
     * caused the interaction.
     */
    public abstract void Interact(GameObject sender);
}
