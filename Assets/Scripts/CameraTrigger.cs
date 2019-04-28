using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraTrigger : MonoBehaviour {

    [Tooltip("The camera associated with this area. If unset, this component will try to find a camera in its children")]
    public Camera areaCamera;

    public enum CameraTriggerBehavior {
        Disabled,
        PassiveEnableFixedCamera,
        JumpToFixedCameraOnFirstInteraction,
        PreviewFixedCameraOnFirstInteraction,
        AlwaysZoomToFixedCamera
    }
    public CameraTriggerBehavior cameraTriggerBehavior;
    private bool hasBeenTriggeredByPlayer = false;
    private CameraScript activeCamera;
    
    [Tooltip("Additional actions to take when player enters this trigger")]
    public UnityEvent onTriggerEnter;
    
    [Tooltip("Additional actions to take when player exits this trigger")]
    public UnityEvent onTriggerExit;

    void Awake() {
        activeCamera = GameObject.Find("MainCamera").GetComponent<CameraScript>();
        if (activeCamera == null) {
            Debug.LogError("CameraTrigger unable to find main camera!");
        }
        if (areaCamera == null) {
            areaCamera = GetComponentInChildren<Camera>();
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Triggered!");
        if (other.GetComponent<PlayerController>() != null) {
            Debug.Log("Triggered by player!");
            if (cameraTriggerBehavior != CameraTriggerBehavior.Disabled) {
                activeCamera.SetFixedCamera(areaCamera);
                if (cameraTriggerBehavior != CameraTriggerBehavior.PassiveEnableFixedCamera &&
                    (cameraTriggerBehavior == CameraTriggerBehavior.AlwaysZoomToFixedCamera || 
                     !hasBeenTriggeredByPlayer))
                {
                    if (cameraTriggerBehavior == CameraTriggerBehavior.PreviewFixedCameraOnFirstInteraction) {
                        activeCamera.PreviewFixedCamera();
                    } else {
                        activeCamera.ZoomToFixedCamera();
                    }
                }
                hasBeenTriggeredByPlayer = true;
            }
            onTriggerEnter.Invoke();
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            if (cameraTriggerBehavior == CameraTriggerBehavior.AlwaysZoomToFixedCamera) {
                activeCamera.ZoomToPlayer();   
            }
            onTriggerExit.Invoke();
        }
    }
}
