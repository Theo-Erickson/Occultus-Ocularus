using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.Input;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float maxWalkSpeed = 1;
    public float walkForce = 20;

    [SerializeField] public PlayerInputMapping playerInput;

    public enum JumpMode { VelocityBased, GravityBased }
    [Header("Jumping")]
    [Tooltip("Is the jump using a velocity function or gravity?")]
    public JumpMode jumpMode;

    // Velocity-Based Jump variables
    public float maxJumpTime = 2.0f;
    public float jump_force = 1200;
    public float jump_offset = 0.0f;
    public float jump_shift = -0.1f;
    public float jump_forced_decel = -50f;
    private bool allowedToJump = false;
    private float jumpTime = 0.0f;

    // Gravity-Based Jump Variables
    [Header("Gravity Jump Mode (Old)")]
    [Tooltip("The initial force to kick the player off the ground.")]
    public float jumpForce = 250;
    [Tooltip("How long in fixedUpdate() iterations to allow the player to keep holding the jump button to go higher (by counteracting gravity)")]
    public int jumpExtensionTime = 15;
    public bool wallJumpingEnabled = false;
    public bool ledgeRecoveryEnabled = false;

    [Header("Audio")]
    public AudioSource backgroundAudio;
    public AudioSource footsteps;

    [Header("Development Features")]
    public bool allowResetting = true;
    public bool allowFlying = true;

    private Vector3 startPoint;
    private SpriteRenderer spriterender;
    private BoxCollider2D playerCollider;
    private BoxCollider2D playerTopTrigger;
    private BoxCollider2D playerBottomTrigger;
    private BoxCollider2D playerRightTrigger;
    private BoxCollider2D playerLeftTrigger;

    private float h; //stores the horizontal axis input value
    //private float prevh; //stores the previous horizontal axis reading
    private bool jump = false;
    private bool prevJump = false;
    private bool jumpStart;

    [HideInInspector]
    public Rigidbody2D body;

    // For determining whether the player is touching the ground
    [HideInInspector]
    public bool touchingGround;
    // TouchingWall will be -1 when the player is contacting a surface to her right and 1 when the wall is to her left (0 when not touching any walls)
    private int touchingWall = 0;
    // For implementing the lerp when you stop moving
    private float stopLerpTime;
    //[HideInInspector] public int flipDirectionTimer;

    private int jumpTimer;
    private int walljumpTimer;
    private bool hangingOnLedge = true;
    private int triggerCount;
    private List<Collider2D> overlappingTriggers = new List<Collider2D>();

    private string currentScene;
    private Animator anim;


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
        anim = GetComponent<Animator>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    void Update() {
        PlayerInputModel.instance.DebugLogInput();
        
        if (this.transform.position.y < -50) { ResetPlayer(); print("RESPAWN"); }

        if (canMove) {
            InputHandler();

            // Flips sprite depending on direction of movement
            h = PlayerInputModel.instance.movement.x;
            if (h > 0)
                spriterender.flipX = true;
            else if (h < 0)
                spriterender.flipX = false;
        }

        // Sets animation variables
        anim.SetFloat("speed", Mathf.Abs(body.velocity.x));
        if (Input.GetButtonDown("Jump") && canMove)
            anim.SetBool("startJump", true);
        else
            anim.SetBool("startJump", false);
        anim.SetBool("grounded", touchingGround);
    }

    // Function to handle button or mouse events to avoid cluttering update
    void InputHandler() {

        // Reset position
        if (allowResetting && Input.GetKeyDown(KeyCode.R)) ResetPlayer();
        // Jump, set variable, force is applied in in FixedUpdate, (recommended by Unity docs).
        jump = PlayerInputModel.instance.jumpPressed;
        jumpStart = !prevJump && jump;

        // If not in antigrav and the player just pressed the jump key:
        if (!antiGrav && jumpMode == JumpMode.VelocityBased && jumpStart && touchingGround)
        {
            // Only jump if touching the ground
            allowedToJump = true;
            jumpTime = 0.0f;
        }


        // Shift between flying and grounded modes
        if (allowFlying && Input.GetKeyDown(KeyCode.RightShift))
        {
            if (antiGrav) {
                this.GetComponent<Renderer>().material.color = Color.white;
                antiGrav = false;
                body.gravityScale = 1;
            } else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
        }
    }

    void PlaySoftFootstep() {
        if (currentScene.Equals("OutsideMall;Rooftops")) {
            footsteps.clip =
                Resources.Load<AudioClip>("Audio/SFX/Characters/Soft Footsteps/SoftFootsteps" + Random.Range(1, 4));
            footsteps.pitch = Random.Range(0.7f, 1.0f);
            footsteps.volume = Random.Range(0.1f, 0.2f);
            footsteps.Play();
        }
    }

    void PlayHardFootstep() {
        if (currentScene.Equals("MallIntro")) {
            footsteps.clip =
                Resources.Load<AudioClip>("Audio/SFX/Characters/Hard Footsteps/footsteps_" + Random.Range(1, 8));
            footsteps.pitch = Random.Range(0.7f, 1.0f);
            footsteps.volume = Random.Range(0.3f, 0.4f);
            footsteps.Play();
        }
    }

    void PlayJumpSound() {
        footsteps.clip = Resources.Load<AudioClip>("Audio/SFX/Characters/jump");
        footsteps.volume = 0.6f;
        footsteps.Play();
    }

    void PlayLandSound() {
        footsteps.clip = Resources.Load<AudioClip>("Audio/SFX/Characters/land");
        footsteps.volume = 0.5f;
        footsteps.Play();
    }

    void StopFootstep() {
        if (footsteps.clip != null && !footsteps.clip.name.Equals("jump"))
            footsteps.Stop();
    }

    void FixedUpdate() {
        if (canMove) {
            // If you are allowed free flight
            if (antiGrav) {
                this.transform.position = this.transform.position +
                                          Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * maxWalkSpeed * 5;
                this.transform.position = this.transform.position +
                                          Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * maxWalkSpeed *
                                          5;
                playerCollider.isTrigger = true;
            } else {
                playerCollider.isTrigger = false; // so the player isn't no-clipping after exiting anti-grav mode

                // -- Jumping Logic: --
                if (jumpMode == JumpMode.VelocityBased) VelocityBasedJump();
                else if (jumpMode == JumpMode.GravityBased) GravityBasedJump();

                prevJump = jump;

                // -- Walking Logic: --
                // prevh = h;
                h = PlayerInputModel.instance.movement.x;

                if (System.Math.Abs(h) < 0.01 && touchingGround) { //when ther'es little-to-no sideways input && we're on the ground bring the player to a stop
                    // increse the stopLerpTime, 
                    stopLerpTime += 3.5f * Time.deltaTime;
                    // slow down the x velocity by an value between the current x velocity and 0, determined by how far along stopLerpTime is, 
                    body.velocity =
                        new Vector2(Mathf.Lerp(body.velocity.x, 0, stopLerpTime),
                            body.velocity.y); //new Vector2(0, 0);//
                } else {
                    // when we start moving, reset stopLerpTime
                    stopLerpTime = 0;
                    if (h * body.velocity.x < maxWalkSpeed) {
                        //if we just did a wall jump (wallJumpTimer > 0), decrese the effectiveness of the players horizontal control, so they can't keep planting themselves back on the wall.
                        if (walljumpTimer != 0)
                            body.AddForce(Vector2.right * h * Mathf.Max(0, walkForce / (walljumpTimer / 2)));
                        else body.AddForce(Vector2.right * h * walkForce);
                    }

                    if (Mathf.Abs(body.velocity.x) > maxWalkSpeed)
                        body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxWalkSpeed, body.velocity.y);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherCol) {
        triggerCount++;
        overlappingTriggers.Add(otherCol);
        WallGroundCheck(otherCol);

        if (otherCol.gameObject.tag == "Platform")
            otherCol.gameObject.GetComponent<MovingPlatform>().StickPlayer(gameObject);
    }

    void OnTriggerExit2D(Collider2D otherCol) {
        triggerCount--;
        overlappingTriggers.Remove(overlappingTriggers.Find(x => x.Equals(otherCol)));
        WallGroundCheck(otherCol);

        if (otherCol.gameObject.tag == "Platform")
            otherCol.gameObject.GetComponent<MovingPlatform>().UnstickPlayer();
    }

    void WallGroundCheck(Collider2D otherCol) {
        touchingGround = false;
        touchingWall = 0;
        hangingOnLedge = false;
        foreach (Collider2D objCollider in overlappingTriggers)
        {
            if (!objCollider.isTrigger)
            {
                if (playerBottomTrigger.IsTouching(objCollider))
                {
                    touchingGround = true;
                    touchingWall = 0;
                    hangingOnLedge = false;
                }
                else if (playerLeftTrigger.IsTouching(objCollider))
                {
                    touchingWall = 1;
                    hangingOnLedge = !playerTopTrigger.IsTouching(otherCol);
                }
                else if (playerRightTrigger.IsTouching(objCollider))
                {
                    touchingWall = -1;
                    hangingOnLedge = !playerTopTrigger.IsTouching(otherCol);
                }
            }
        }
    }

    private void ResetPlayer() {
        this.transform.position = startPoint;
        this.GetComponent<PlayerLayerSwitcher>().SwitchPlayerLayer(LayerMask.NameToLayer("Foreground"));
    }

    private void VelocityBasedJump() {
        if (jump && jumpTime <= maxJumpTime && allowedToJump)
        {
            //float coef = 0.0f;
            //float force = 0.0f;

            float desiredSpeed = 2 / (0.5f + Mathf.Pow(5, 20 * (jumpTime + jump_shift))) + jump_offset;
            //float desiredSpeed = 1 / Mathf.Pow(jumpTime-1,10)-0.5f;
            desiredSpeed *= jump_force;
            float currentSpeed = this.GetComponent<Rigidbody2D>().velocity.y;
            float dif = desiredSpeed - currentSpeed;
            dif -= Physics.gravity.y;

            body.AddForce(dif * this.GetComponent<Rigidbody2D>().mass * Vector3.up * Time.deltaTime);
            /*
            //coef = 2 / (1 + Mathf.Pow(50, (jumpTime + 1)));
            //coef = Mathf.Sqrt(coef);
            coef = 1 / Mathf.Pow(jumpTime+1, 2) + 0.3f;
            coef*=this.GetComponent<Rigidbody2D>().mass;
            if (jumpTime <= 0.3f)
            {
                coef = 1;
                force = jumpForce;
            }
            body.AddForce(force * coef * Vector3.up);
            */
            jumpTime += Time.deltaTime;
        }
        else if (!touchingGround)
        {
            allowedToJump = false;
            if (this.GetComponent<Rigidbody2D>().velocity.y >= 0)
            {
                body.AddForce(jump_forced_decel * this.GetComponent<Rigidbody2D>().mass * Vector3.up);
            }
        }
    }

    private void GravityBasedJump() {
        if (touchingGround)
        {
            walljumpTimer = 0;
            // if the jump was just started
            if (jumpStart)
            {
                jumpTimer = jumpExtensionTime;
                body.AddForce(jumpForce * Vector3.up);
            }
        }
        else
        {
            if (jump && jumpTimer != 0)
            {
                body.AddForce(-Physics2D.gravity -
                              (1 / jumpTimer) *
                              Vector2.up); // counteract gravity while the user is holding the jump button & jump timer hasn't gotten to zero.
                jumpTimer--;
            }
            else
            {
                jumpTimer = 0;
            }

            if (wallJumpingEnabled)
            {
                if (jumpStart && touchingWall != 0)
                {
                    walljumpTimer = 60;
                    jumpTimer = jumpExtensionTime;
                    if (!hangingOnLedge) body.velocity = new Vector2(touchingWall * 4, 4);
                }

                if (walljumpTimer != 0) walljumpTimer--;
            }

            if (ledgeRecoveryEnabled && jumpStart && hangingOnLedge && body.velocity.y < 1)
            {
                body.velocity = new Vector2(-touchingWall, 4);
            }
        }
    }
}