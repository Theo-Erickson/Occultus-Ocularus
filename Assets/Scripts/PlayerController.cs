using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float maxWalkSpeed = 1;
    public float walkForce = 20;
    public float jumpForce = 20;
    [Tooltip("How long in fixedUpdate() iterations to allow the player to keep holding the jump button to go higher")]
    public int jumpExtensionTime = 20;

    [Header("Audio")]
    public AudioSource backgroundAudio;
    public AudioSource footsteps;

    private Rigidbody2D body;
    private Vector3 startPoint;
    private SpriteRenderer spriterender;
    private BoxCollider2D playerCollider;
    private BoxCollider2D playerTopTrigger;
    private BoxCollider2D playerBottomTrigger;
    private BoxCollider2D playerRightTrigger;
    private BoxCollider2D playerLeftTrigger;

    private float h; //stores the horizontal axis input value
    private float prevh; //stores the previous horizontal axis reading
    private bool jump = false;
    private bool prevJump = false;
    // For determining whether the player is touching the ground

    //private ContactPoint2D[] contactPoints = new ContactPoint2D[5];
    // private int contactCount = 1;
    [HideInInspector]
    public bool touchingGround;
    // TouchingWall will be -1 when the player is contacting a surface to her right and 1 when the wall is to her left (0 when not touching any walls)
    private int touchingWall = 0;
    // For implementing the lerp when you stop moving
    private float stopLerpTime;
    [HideInInspector]
    public int flipDirectionTimer;

    private int jumpTimer;
    private int walljumpTimer;
    private bool hangingOnLedge = true;
    private int triggerCount;
    private List<Collider2D> overlapingTriggers = new List<Collider2D>();


    // Use this for initialization
    void Start() {
        startPoint = this.transform.position;
        body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
        playerCollider = GetComponents<BoxCollider2D>()[0];
        playerTopTrigger = GetComponents<BoxCollider2D>()[1];
        playerBottomTrigger = GetComponents<BoxCollider2D>()[2];
        playerRightTrigger = GetComponents<BoxCollider2D>()[3];
        playerLeftTrigger = GetComponents<BoxCollider2D>()[4];
    }

    void Update() {
        if (this.transform.position.y < -50) { this.transform.position = startPoint; print("RESPAWN"); }


        // Flips sprite depending on direction of movement
        if (Input.GetAxis("Horizontal") < 0) {
            spriterender.flipX = true;
            //  transform.eulerAngles = new Vector3(0, 0, 1); //slight angle allows ledge detection to work.
        }
        else if (Input.GetAxis("Horizontal") > 0) {
            spriterender.flipX = false;
            //  transform.eulerAngles = new Vector3(0, 0, -1);
        }

        InputHandler();
        AudioHandler();
    }

    // Function to handle button or mouse events to avoid cluttering update
    void InputHandler() {


        // Reset position
        if (Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = Vector3.zero;
        }

        // Jump, set variable, force is applied in in FixedUpdate, (recommended by Unity docs).
        jump = Input.GetButton("Jump");
        // If you can move freely, switch camera modes
        if (jump && antiGrav) {
            CameraScript cScript = Camera.main.GetComponent<CameraScript>();
            if (cScript.mode == CameraScript.CameraMode.Fixed) {
                cScript.mode = CameraScript.CameraMode.FollowPlayer;
            }
            else if (cScript.mode == CameraScript.CameraMode.FollowPlayer) {
                cScript.mode = CameraScript.CameraMode.Fixed;
                cScript.SetDestination(cScript.pastCameraPosition, 2.0f);
            }
        }

        // Shift between flying and grounded modes
        if (Input.GetKeyDown(KeyCode.RightShift)) {
            if (antiGrav) {
                this.GetComponent<Renderer>().material.color = Color.white;
                antiGrav = false;
                body.gravityScale = 1;
            }
            else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
        }
    }


    void AudioHandler() {
        //old implementation: for (int i = 0; i < contactCount; i++) {
        // old implementation: if (Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.5) {
        // Only make sound if on the ground & If horizontally moving.
        if (touchingGround && Mathf.Abs(body.velocity.x) > 0.1 && !footsteps.isPlaying) {
            footsteps.clip = Resources.Load<AudioClip>("Audio/Footsteps/SoftFootsteps" + Random.Range(1, 4));
            footsteps.pitch = Random.Range(0.7f, 1.0f);
            footsteps.volume = Random.Range(0.1f, 0.2f);
            footsteps.Play();
        }
    }

    void FixedUpdate() {
        if (canMove) {
            // old method: contactCount = playerCollider.GetContacts(contactPoints);
            // If you are allowed free flight
            if (antiGrav) {
                this.transform.position = this.transform.position + Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * maxWalkSpeed;
                this.transform.position = this.transform.position + Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * maxWalkSpeed;
                playerCollider.isTrigger = true;
            }
            // When u walkin / jumpin
            else {
                playerCollider.isTrigger = false;
                // -- Jumping Logic: --

                bool jumpStart = !prevJump && jump;
                if (touchingGround) {
                    walljumpTimer = 0;
                    // if the jump was just started
                    if (jumpStart) {
                        jumpTimer = jumpExtensionTime;
                        body.AddForce(jumpForce * Vector3.up);
                    }
                }
                else {
                    if (jump && jumpTimer != 0) {
                        body.AddForce(-Physics2D.gravity - (1 / jumpTimer) * Vector2.up);
                        jumpTimer--;
                    }
                    if (jumpStart && touchingWall != 0) {
                        walljumpTimer = 60;
                        jumpTimer = jumpExtensionTime;
                        if (hangingOnLedge) {
                            print("here");
                            body.velocity = new Vector2(-touchingWall, 4);
                        }
                        else body.velocity = new Vector2(touchingWall * 4, 4);
                    }
                    if (walljumpTimer != 0) walljumpTimer--;
                }

                // -- Walking Logic: --

                prevh = h;
                h = Input.GetAxis("Horizontal");

                prevJump = jump;

                if (System.Math.Abs(h) < 0.01 && touchingGround) { //when ther'es little-to-no sideways input && we're on the ground bring the player to a stop
                    // increse the stopLerpTime, 
                    stopLerpTime += 3.5f * Time.deltaTime;
                    // slow down the x velocity by an value between the current x velocity and 0, determined by how far along stopLerpTime is, 
                    body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, stopLerpTime), body.velocity.y);//new Vector2(0, 0);//
                }
                //else if (flipDirectionTimer > 0) {
                //    flipDirectionTimer--;
                //    if (touchingGround) {
                //        // increse the stopLerpTime, 
                //        stopLerpTime += 3.5f * Time.deltaTime;
                //        // slow down the x velocity by an value between the current x velocity and 0, determined by how far along stopLerpTime is, 
                //        body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, stopLerpTime), body.velocity.y);
                //    }
                //}
                else {
                    // when we start moving, reset stopLerpTime
                    stopLerpTime = 0;
                    if (h * body.velocity.x < maxWalkSpeed) {
                        //if we just did a wall jump (wallJumpTimer > 0), decrese the effectiveness of the players horizontal control, so they can't keep planting themselves back on the wall.
                        if (walljumpTimer != 0) body.AddForce(Vector2.right * h * Mathf.Max(0, walkForce / (walljumpTimer / 2)));
                        else body.AddForce(Vector2.right * h * walkForce);
                    }
                    if (Mathf.Abs(body.velocity.x) > maxWalkSpeed) body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxWalkSpeed, body.velocity.y);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherCol) {
        triggerCount++;
        //overlapingTriggers;
        checkTriggers(otherCol, true);

    }

    void OnTriggerExit2D(Collider2D otherCol) {
        triggerCount--;
        //overlapingTriggers;
        checkTriggers(otherCol, false);

    }

    void checkTriggers(Collider2D otherCol, bool isEnter) {
        if (isEnter) {
            overlapingTriggers.Add(otherCol);
        }
        else {
            overlapingTriggers.Remove(overlapingTriggers.Find(x => x.Equals(otherCol)));
        }
        Debug.Log(overlapingTriggers.ToString());
        touchingGround = false;
        touchingWall = 0;
        hangingOnLedge = false;
        foreach (Collider2D objCollider in overlapingTriggers) {
            if (playerBottomTrigger.IsTouching(objCollider)) {
                touchingGround = true;
                touchingWall = 0;
                hangingOnLedge = false;
                if (isEnter && otherCol.gameObject.tag == "Platform") otherCol.gameObject.GetComponent<MovingPlatform>().stickPlayer(gameObject);
            }
            else if (playerLeftTrigger.IsTouching(objCollider)) {
                touchingWall = 1;
                hangingOnLedge = !playerTopTrigger.IsTouching(otherCol);
            }
            else if (playerRightTrigger.IsTouching(objCollider)) {
                touchingWall = -1;
                hangingOnLedge = !playerTopTrigger.IsTouching(otherCol);
            } else if (!isEnter && otherCol.gameObject.tag == "Platform") {
                otherCol.gameObject.GetComponent<MovingPlatform>().unStickPlayer(); ;
            }
            //print(touchingWall + " G: " + touchingGround + " h:" + hangingOnLedge);
        }
    }
}