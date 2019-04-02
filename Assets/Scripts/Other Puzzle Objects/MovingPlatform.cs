using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to move platforms in any of these three ways:
 *   1. A continuous back and forth pattern without stopping
 *   2. A continuous back and forth pattern that can be started and
 *      stopped at any time with the StartMoving() and StopMoving() functions
 *   3. Extended and retracted with the Extend() and Retract() functions
 * 
 * The distance and speed of movement are configured with the moveDistance and
 * moveSpeed fields, and the orientation of movement can be set to horizontal
 * or vertical with the movementDirection field.
 */

public class MovingPlatform : MonoBehaviour
{

    [Tooltip("If true, platform will move on its own")]
    public bool move;

    [Header("Movement Mode")]
    [Tooltip("True: the platform shifts between extended and retracted positions, stopping at " +
    	"the end of its track until it is told to extend or retract away from its current position. " +
    	"False: the platform moves continuously, with a pause at either end of [pause time] seconds.")]
    public bool extendAndRetract;
    [Tooltip("If moving continuously, will pause at each end for this many seconds")]
    public float pauseTime;

    [Header("Movement Properties")]
    [Tooltip("Distance platform moves, can be positive or negative")]
    public float distance;
    [Tooltip("The platform's speed in m/s")]
    public float speed;
    public enum MvDir {horizontal, vertical};
    [Tooltip("Make platform move side-to-side or up-and-down")]
    public MvDir direction;

    private bool extend;
    private float back;
    private float front;
    private bool inverted;
    private float displacement;
    private bool forwards;
    private Vector3 originalPosition;

    private float timePaused;
    private bool paused;
    private bool canPause;

    float platformPosition;
    GameObject collidingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.position;
        displacement = speed / 60;
        if (distance < 0)
            inverted = true;

        // Set front and back bounds for movement
        if (!inverted)
        {
            if (direction == MvDir.horizontal)
            {
                back = originalPosition.x;
                front = originalPosition.x + distance;
            }
            else if (direction == MvDir.vertical)
            {
                back = originalPosition.y;
                front = originalPosition.y + distance;
            }
            extend = false;
        }
        else if (inverted)
        {
            if (direction == MvDir.horizontal)
            {
                back = originalPosition.x + distance;
                front = originalPosition.x;
            }
            else if (direction == MvDir.vertical)
            {
                back = originalPosition.y + distance;
                front = originalPosition.y;
            }
        }
        extend = true;
    }

    void FixedUpdate()
    {
        if (paused)
            PauseMoving();

        if (move && Mathf.Abs(distance) > 0)
        {

            if (direction == MvDir.horizontal)
            {
                // If at bound, turn around or stop if in extendAndRetract mode
                if (transform.position.x >= front)
                {
                    if (pauseTime > 0 && canPause)
                        PauseMoving();
                    forwards = false;
                    if (extendAndRetract && extend)
                        move = false;
                }
                else if (transform.position.x <= back)
                {
                    if (pauseTime > 0 && canPause)
                        PauseMoving();
                    forwards = true;
                    if (extendAndRetract && !extend)
                        move = false;
                }

                // If going wrong direction, turn around
                if (extendAndRetract)
                {
                    if (!inverted && !forwards && extend)
                        forwards = true;
                    else if (inverted && forwards && !extend)
                        forwards = false;
                }

                // Move right or left
                if (move)
                {
                    if (!extendAndRetract)
                        canPause = true;
                    if (!inverted)
                    {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x - displacement, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        if (!forwards && (!extendAndRetract || !extend))
                            transform.position = new Vector3(transform.position.x - displacement, transform.position.y, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
                    }
                    if (collidingPlayer != null) {
                        collidingPlayer.transform.Translate(new Vector3(transform.position.x - platformPosition, 0, 0));
                        platformPosition = transform.position.x;
                    }
                }
            }
            else if (direction == MvDir.vertical)
            {
                // If at bound, turn around or stop if in extendAndRetract mode
                if (transform.position.y >= front)
                {
                    if (pauseTime > 0 && canPause)
                        PauseMoving();
                    forwards = false;
                    if (extendAndRetract && extend)
                        move = false;
                }
                else if (transform.position.y <= back)
                {
                    if (pauseTime > 0 && canPause)
                        PauseMoving();
                    forwards = true;
                    if (extendAndRetract && !extend)
                        move = false;
                }

                // If going wrong direction, turn around
                if (extendAndRetract)
                {
                    if (!inverted && !forwards && extend)
                        forwards = true;
                    else if (inverted && forwards && !extend)
                        forwards = false;
                }

                // Move up or down
                if (move)
                {
                    if (!extendAndRetract)
                        canPause = true;
                    if (!inverted)
                    {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x, transform.position.y + displacement, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y - displacement, transform.position.z);
                    }
                    else
                    {
                        if (!forwards && (!extendAndRetract || !extend))
                            transform.position = new Vector3(transform.position.x, transform.position.y - displacement, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y + displacement, transform.position.z);
                    }
                    if (collidingPlayer != null) {
                        collidingPlayer.transform.Translate(new Vector3(0, transform.position.y - platformPosition, 0));
                        platformPosition = transform.position.y;
                    }
                }
            }
        }
    }

    private void PauseMoving()
    {
        if (timePaused < pauseTime)
        {
            paused = true;
            extendAndRetract = true;
            timePaused += Time.deltaTime;
            move = false;
        }
        else
        {
            paused = false;
            extendAndRetract = false;
            timePaused = 0;
            move = true;
            canPause = false;
        }

    }

    public void StartMoving()
    {
        extendAndRetract = false;
        move = true;
    }

    public void StopMoving()
    {
        extendAndRetract = false;
        move = false;
    }

    public void Extend()
    {
        extendAndRetract = true;
        move = true;
        paused = false;
        canPause = false;
        if (!inverted)
            extend = true;
        else
            extend = false;
    }

    public void Retract()
    {
        extendAndRetract = true;
        move = true;
        paused = false;
        canPause = false;
        if (!inverted)
            extend = false;
        else
            extend = true;
    }

    public void ExtendOrRetract()
    {
        move = true;
        paused = false;
        canPause = false;
        extend = !extend;
    }

    // Called by player class when they jump or walk onto platform: Makes player stick to platform when they're standing on it.
    public void StickPlayer(GameObject player) {
        collidingPlayer = player;
        if (direction == MvDir.vertical) {
            platformPosition = transform.position.y;
        }
        else if (direction == MvDir.horizontal) {
            platformPosition = transform.position.x;
        }
    }

    // Called by player class when they jump or walk off of platform (Unsticks player from platform)
    public void UnstickPlayer() {
        collidingPlayer = null;
    }
}
