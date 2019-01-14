using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This interaction will enable or disable the given GameObject.
 */
public class InteractionToggle : Interaction
{
    [Tooltip(
     "This is the game object that will be enabled or disabled. "
     + "If it is set to null, this object itself will be toggled. "
     + "This is useful if for example, the object that should be toggled "
     + "is a child of some other object that should handle the interactions.")]
    public GameObject toggledObject = null;

    public enum Mode {
        Toggle,
        AlwaysDisable,
        AlwaysEnable
    }

    [Tooltip("Whether the object should act as a toggle or not.")]
    public Mode mode = Mode.Toggle;

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
        if(toggledObject != null)
        {
            switch(mode)
            {
                case Mode.Toggle:
                    toggledObject.SetActive(!toggledObject.activeSelf);
                    break;
                case Mode.AlwaysDisable:
                    toggledObject.SetActive(false);
                    break;
                case Mode.AlwaysEnable:
                    toggledObject.SetActive(true);
                    break;
            }
        }
        else
        {
            switch(mode)
            {
                case Mode.Toggle:
                    gameObject.SetActive(!gameObject.activeSelf);
                    break;
                case Mode.AlwaysDisable:
                    gameObject.SetActive(false);
                    break;
                case Mode.AlwaysEnable:
                    gameObject.SetActive(true);
                    break;
            }
        }
    }
}
