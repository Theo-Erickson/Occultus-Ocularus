using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraMode { Fixed, FollowPlayer, FollowPlayerRadius, FollowPlayerSmooth, FreeCam }

    [Header("Camera Mode")]
    [Tooltip("Does the camera stay still, follow the player, or follow within a radius?")]
    public CameraMode mode;

    [Header("Positioning")]
    [Tooltip("Used so the camera knows where it came from")]
    public Vector3 start;
    [Tooltip("Tells the camera where to go")]
    public Vector3 destination;

    public Vector3 pastCameraPosition;

    [Tooltip("How fast the camera should \"catch up\" with the player when in follow radius mode.")]
    public float followSpeed = 10;

    private GameObject player;
    private Rigidbody2D playerRigidbody;
    private float timeToReachTarget = 5.0f;
    private float t;

    // Stores the z(into the screen) distance between the camera and player (calculated when the game starts from (camera z - player z) coordinates)
    private float CameraToPlayerZDistance;

    // these only apply to the FollowPlayerRadius game mode:
    [Header("FollowPlayerRadius")]
    [Tooltip("How far the player can move from the view center before the camera will follow it (Applies only to the FollowPlayerRadius camera mode)")]
    public float followRadius = 3;
    // Stores the coordinates representing the center of the camera view in world space (2d)
    private Vector2 screenCenterVector;
    // Stores the vector between the center of the camera view in world space to the player's position (2d).")]
    private Vector2 screenToPlayerVector;

    // these only apply to the FollowPlayerSmooth game mode:
    [Header("FollowPlayerSmooth")]
    [Tooltip("When the player isn't moving vertically, how far below the center of the screen should the camera show the player")]
    public float playerRestYOffset = 1;
    [Tooltip("The Y velocity of the player at which the camera starts tracking the player in the center of the screen rather than at the 'playerRestYOffset'")]
    public float playerRestThreshold = 1;
    [Tooltip("(Higher = slower) When at player is not at rest, determines how slow the camera should \"catch up\" with the target postiton when in follow smooth mode.")]
    public float followSmoothSpeedX = 2.5f;
    [Tooltip("(Higher = slower) When at player is not at rest, determines how slow the camera should \"catch up\" with the target postiton when in follow smooth mode.")]
    public float followSmoothSpeedY = 2;
    [Tooltip("(Higher = slower) When at player is at rest, determines how slow the camera should \"catch up\" with the target postiton when in follow smooth mode.")]
    public float followSmoothRestSpeed = 40;
    private int cameraGroundMovementPause;
    public int movingTimerXMax = 60;
    private int movingTimerY;
    public int movingTimerYMax = 60;
    private Vector2 cameraMovingTarget;
    private PlayerController playerScript;
    float xOffset = 0;
    float yOffset = 0;

    [Header("FreeCam")]
    public float freeMoveSpeed = 0.2f;
    public Vector2 DistFromPlayer;
    public Vector2 MaxDistFromPlayer = new Vector2(10, 10);
    float desiredCameraTargetX;
    float desiredCameraTargetY;


    void Start() {
        pastCameraPosition = start;
        player = GameObject.Find("Player");
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerController>();
        start = destination = transform.position;
        CameraToPlayerZDistance = transform.position.z - player.transform.position.z;
        cameraMovingTarget = player.transform.position;
    }

    void Update() {
        //check if camera mode should change
        CheckCameraMode();


        //Camera is a set a point and does not travel with player
        if (mode == CameraMode.Fixed) {
            //remove this camera being parented to the child. Disables direct following
            if (this.transform.parent == player.transform) {
                this.transform.parent = null;
            }
            LerpToPosition(this.transform.position, destination);

            //Camera is set to follow player
        }
        else if (mode == CameraMode.FollowPlayer) {

            //So unity only makes it happen once for better performance
            if (this.transform.parent != player.transform) {
                this.transform.parent = player.transform;
            }

            //Camera follows player with a radius buffer
        }
        else if (mode == CameraMode.FollowPlayerRadius) {

            //do normal stuff
            //get the center of the screen (mainCamera) in the unity world units
            screenCenterVector = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2), (Screen.height / 2)));
            screenToPlayerVector = (Vector2)player.transform.position - screenCenterVector; //subtracts the center of the screen coordinates (2d) from the player's coordinates;
            if (screenToPlayerVector.magnitude > followRadius) // if the player is too far from the center of the view:
            {
                //screenToPlayerVector.z = (transform.position.z - player.transform.position.z) - CameraToPlayerZDistance;
                //move the camera such that the player is within the radius focusAreaSize of the view center again:
                transform.Translate((screenToPlayerVector - screenToPlayerVector.normalized * followRadius) * Time.deltaTime * followSpeed, Space.World);
            }
            //player can control camera with arrow keys
        }
        else if (mode == CameraMode.FreeCam) {
            //move left and right
            if (Input.GetKey(KeyCode.RightArrow) && DistFromPlayer.x < MaxDistFromPlayer.x) {
                this.transform.position = new Vector3(this.transform.position.x + freeMoveSpeed, this.transform.position.y, this.transform.position.z);
                DistFromPlayer.x += freeMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && DistFromPlayer.x > -MaxDistFromPlayer.x) {
                this.transform.position = new Vector3(this.transform.position.x - freeMoveSpeed, this.transform.position.y, this.transform.position.z);
                DistFromPlayer.x -= freeMoveSpeed;
            }


            if (Input.GetKey(KeyCode.UpArrow) && DistFromPlayer.y < MaxDistFromPlayer.y) {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + freeMoveSpeed, this.transform.position.z);
                DistFromPlayer.y += freeMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && DistFromPlayer.y > -MaxDistFromPlayer.y) {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - freeMoveSpeed, this.transform.position.z);
                DistFromPlayer.y -= freeMoveSpeed;
            }
        }
        // This updates the Z(into screen) position of the camera when the player switches layers, 
        // which I think should happen for all camera modes, because it helps the player notice which layer they are on.
        if ((transform.position.z - player.transform.position.z) != CameraToPlayerZDistance) {//warning is ok, because we keep updating the camera z distance each frame to be closer to the correct one;
            transform.Translate(new Vector3(0, 0, CameraToPlayerZDistance - (transform.position.z - player.transform.position.z)) * Time.deltaTime * followSpeed, Space.World);
        }
    }


    //NOT IN USE RIGHT NOW
    // Makes the camera Lerp from it's current position to another and stops character from moving
    private void LerpToPosition(Vector3 start, Vector3 end) {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(start, end, t);
        print("Lerp");
        //Disable the player movement when the camera is shifting
        if (Vector3.Distance(transform.position, end) > 1) {
            player.GetComponent<PlayerController>().canMove = false;
            //Stop the player from moving when you are transitioning with the camera 
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else {
            player.GetComponent<PlayerController>().canMove = true;
            //Give player the contraints on all rotations + Z axis position
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //Check for conditions to change camera mode
    public void CheckCameraMode() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            mode = CameraMode.Fixed;
            DistFromPlayer = Vector2.zero;
            player.GetComponent<PlayerController>().canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            mode = CameraMode.FollowPlayer;
            DistFromPlayer = Vector2.zero;
            player.GetComponent<PlayerController>().canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            mode = CameraMode.FollowPlayerRadius;
            DistFromPlayer = Vector2.zero;
            player.GetComponent<PlayerController>().canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            mode = CameraMode.FreeCam;
            player.GetComponent<PlayerController>().canMove = false;
        }
    }

    public void resetPosition(Vector3 destination, float time) {
        t = 0;
        start = transform.position;
        timeToReachTarget = time;
        destination = start;
    }

    //Use to tell the camera where to move to in fixed mode
    public void SetDestination(Vector3 destination, float time) {
        t = 0;
        start = transform.position;
        timeToReachTarget = time;
        this.destination = destination;
    }

    public void FixedUpdate() {
        // putting the camera following in fixed update keeps jitter between the player and camera low.
        if (mode == CameraMode.FollowPlayerSmooth) {
            //get the center of the screen (mainCamera) in the unity world units:
            screenCenterVector = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2), (Screen.height / 2)));
            //subtracts the center of the screen coordinates (2d) from the player's coordinates;
            screenToPlayerVector = (Vector2)player.transform.position - screenCenterVector;

            float xInput = Input.GetAxis("Horizontal");
            float deltaX, deltaY;
            float foo = 1.5f;

            float off = 0;
            // Figure out Y-axis camera motion (deltaY):
            if (playerScript.touchingGround) {
                movingTimerY = 0;
                off = 2;
                if (cameraGroundMovementPause < 20) cameraGroundMovementPause++;
                if (desiredCameraTargetY < playerRestYOffset && cameraGroundMovementPause > 19) {
                    desiredCameraTargetY += 0.03f;
                }
            }
            else {
                cameraGroundMovementPause = 0;
                if (movingTimerY < movingTimerYMax) movingTimerY++;
                if (playerRigidbody.velocity.y < 0 && desiredCameraTargetY > -foo) {
                    desiredCameraTargetY -= 0.04f;
                } else 
                if (playerRigidbody.velocity.y > 0 && desiredCameraTargetY < foo) {
                    desiredCameraTargetY += 0.08f;
                }
            }

            print("player: " + playerRigidbody.velocity.y + " counter: " + movingTimerY);

           if (playerRigidbody.velocity.y < -8) off = (-playerRigidbody.velocity.y - 8);
            deltaY = (player.transform.position.y + desiredCameraTargetY - screenCenterVector.y) * Time.deltaTime * (followSmoothSpeedY + off);//(6 / (Mathf.Abs(playerRigidbody.velocity.y)+1)));//* Mathf.Clamp(Mathf.Abs(player.transform.position.y - screenCenterVector.y), 0.1f, 3));

            // Figure out X-axis camera motion (deltaX):
            if (Mathf.Abs(player.transform.position.x + (int)xInput - screenCenterVector.x) > 0.02) {
                if ((int)xInput > 0.01 && desiredCameraTargetX < 1.5) desiredCameraTargetX = desiredCameraTargetX + 0.02f;
                else if ((int)xInput < -0.01 && desiredCameraTargetX > -1.5) desiredCameraTargetX = desiredCameraTargetX - 0.02f;
            }
            if ((int)xInput == 0) {
                if (desiredCameraTargetX > 0.04) desiredCameraTargetX = desiredCameraTargetX - 0.04f;
                if (desiredCameraTargetX < -0.04) desiredCameraTargetX = desiredCameraTargetX + 0.04f;
                deltaX = (player.transform.position.x + desiredCameraTargetX - screenCenterVector.x) * Time.deltaTime * (followSmoothSpeedX + Mathf.Clamp(Mathf.Abs(player.transform.position.x - screenCenterVector.x), 0f, 10)+1);
            }
            else {
                deltaX = (player.transform.position.x + desiredCameraTargetX - screenCenterVector.x) * Time.deltaTime * (followSmoothSpeedX + Mathf.Clamp(Mathf.Abs(player.transform.position.x - screenCenterVector.x), 0f, 10));
            }
            //   Mathf.Clamp(playerRigidbody.velocity.y, 0, 1.5f)
            //Vector2 desiredCameraTarget = new Vector2(Mathf.Sign(xInput)*Mathf.Clamp(Mathf.Pow(movingTimer,4)/Mathf.Pow(movingTimerMax, 4),-1,1), Mathf.Clamp(playerRigidbody.velocity.y, 0, 1.5f));
            // if the player is on the ground, she probably want's to see the most in the up direction (rather than centering the player in the screen):
            // Debug Visualization:
           // GameObject.Find("Debug dot green").transform.position = new Vector2(desiredCameraTargetX, desiredCameraTargetY) + (Vector2)player.transform.position;
            //GameObject.Find("Debug dot brown").transform.position = cameraMovingTarget + (Vector2)player.transform.position;

            // Finally actually move the camera to the players positon plus the camera target offset:
            transform.Translate(new Vector2(deltaX, deltaY), Space.World);
            //transform.Translate(new Vector2((player.transform.position.x + desiredCameraTargetX - screenCenterVector.x) * Time.deltaTime * (followSmoothSpeedX + Mathf.Abs(desiredCameraTargetX)), (player.transform.position.y - screenCenterVector.y) * Time.deltaTime * followSmoothSpeedY), Space.World);



        }
    }
}
