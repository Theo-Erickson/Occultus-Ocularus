using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

/*
 * This interactor causes an interaction when something enters or exits its trigger and the player presses 'Interact'.
 */
public class InteractorButton : MonoBehaviour, IInteractionActions {
    public PlayerInputMapping playerInput;    

    [Tooltip("Objects with this tag will cause interactions. If tag is empty, all objects will cause interactions.")]
    public string objectTag = "Player";
    [Tooltip("Whether or not only objects residing in the same layer as this object will cause interactions.")]
    public bool sameLayerOnly = true;
    public bool onGroundOnly;
    [Tooltip("The event to be triggered when the button is interacted with.")]
    public UnityEvent onInteract;

    [Header("Audio")]
    public AudioSource interactSound;

    [Header("Animation (set to enable animation)")]
    public Animator anim;
    public bool toggleAnimation;


    private bool buttonIsUp = true;
    private float startYpos;


    private List<GameObject> inTrigger = new List<GameObject>();
    private bool triggered = false;

    void Awake() {
        playerInput.Interaction.SetCallbacks(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        interactSound = this.GetComponent<AudioSource>();
        startYpos = transform.position.y;
    }
    public void OnInteract(InputAction.CallbackContext context) {
        if (context.performed) {
            foreach (GameObject go in inTrigger) {
                // If no tag is specified or the object's tag matches the specified tag.
                if (string.IsNullOrEmpty(objectTag) || go.tag == objectTag) {
                    // If sameLayerOnly is false or the object is in the same layer.
                    if (!sameLayerOnly || go.gameObject.layer == gameObject.layer) {
                        if (!onGroundOnly || (go.GetComponent<PlayerController>() != null && go.GetComponent<PlayerController>().touchingGround)) {
                            triggered = true;
                            if (interactSound != null && interactSound.enabled) {
                                interactSound.clip = Resources.Load<AudioClip>("Audio/SFX/UI/Menu-Click");
                                interactSound.pitch = 0.7f;
                                interactSound.volume = 1.0f;
                                interactSound.Play();
                            }
                            if (anim) {
                            transform.position = new Vector3(transform.position.x, startYpos, transform.position.z);
                            if (!toggleAnimation) anim.Play("ButtonPush", 0, 0);
                            else {
                                if (buttonIsUp) {
                                    buttonIsUp = false;
                                    anim.Play("ButtonDown");
                                }
                                else {
                                    transform.position = new Vector3(transform.position.x, startYpos -0.3f, transform.position.z);
                                    buttonIsUp = true;
                                    anim.Play("ButtonUp");
                                }
                            }
                            }
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (triggered == true) {
            onInteract.Invoke();
            triggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't do anything if the other collider is a trigger.
        if (other.isTrigger)
            return;

        inTrigger.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.isTrigger)
            return;

        inTrigger.Remove(other.gameObject);
    }
}
