using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

/*
 * This interactor causes an interaction when something enters or exits its trigger and the player presses 'Interact'.
 */
public class InteractorButton : MonoBehaviour
{
    [Tooltip("Objects with this tag will cause interactions. If tag is empty, all objects will cause interactions.")]
    public string objectTag = "Player";
    [Tooltip("Whether or not only objects residing in the same layer as this object will cause interactions.")]
    public bool sameLayerOnly = true;
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

    // Start is called before the first frame update
    void Start()
    {
        interactSound = this.GetComponent<AudioSource>();
        startYpos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact")) {
            foreach (GameObject go in inTrigger) {
                // If no tag is specified or the object's tag matches the specified tag.
                if (string.IsNullOrEmpty(objectTag) || go.tag == objectTag) {
                    // If sameLayerOnly is false or the object is in the same layer.
                    if (!sameLayerOnly || go.gameObject.layer == gameObject.layer) {
                        triggered = true;
                        if (interactSound != null && interactSound.enabled) {
                            interactSound.clip = Resources.Load<AudioClip>("Audio/SFX/UI/Menu-Click");
                            interactSound.pitch = 0.7f;
                            interactSound.volume = 1.0f;
                            interactSound.Play();
                        }
                        if (anim) {
                            if (!toggleAnimation)
                                anim.Play("ButtonPush", 0, 0);
                            else {
                                if (buttonIsUp) {
                                    buttonIsUp = false;
                                    transform.position = new Vector3(transform.position.x, startYpos, transform.position.z);
                                    anim.Play("ButtonDown");
                                }
                                else {
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        inTrigger.Remove(other.gameObject);
    }
}
