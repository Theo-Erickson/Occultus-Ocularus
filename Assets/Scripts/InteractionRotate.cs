using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This interaction rotates its object by a set amount of degrees.
 */
public class InteractionRotate : Interaction
{
    [Tooltip("How much to rotate the object by.")]
    public int degrees = 45;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject sender)
    {
        transform.Rotate(new Vector3(0, 0, degrees));
    }
}
