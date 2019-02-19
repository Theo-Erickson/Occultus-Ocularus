using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerLayerSwitcher : MonoBehaviour
{
    public GameObject player;
    public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    public LayerMask foregroundLayer;
    public LayerMask midgroundLayer;
    private BoxCollider2D playerCollider;
    private int currentPlayerLayer;
    private bool collisionPlayerFeedbackHappening = false;
    private float collisionPlayerFeedbackZPos;
    private float collisionPlayerFeedbackStartZPos;
    private LayerMask overlapLayerMask;
    private Collider2D[] detectedOverlaps;

    //call this function to switch the player to a layer (foreground, background, etc...)
    public void SwitchPlayerLayer(int destinationLayerNum)
    {
        if (destinationLayerNum != currentPlayerLayer) {
            overlapLayerMask = LayerMask.GetMask(LayerMask.LayerToName(destinationLayerNum));
            // detects when the player will overlap/ collide with the layer they are attempting to switch to and returns (exits function without doing anything);
            if (Physics2D.OverlapCircle(player.transform.position, (float)0.1, overlapLayerMask) != null)
            {
                collisionPlayerFeedbackHappening = true;
                collisionPlayerFeedbackStartZPos = player.transform.position.z;
                if (destinationLayerNum == foregroundLayer.value) player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - (float)0.5);
                if (destinationLayerNum == midgroundLayer.value) player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + (float)0.5);
            }
            else
            {
                if (destinationLayerNum == foregroundLayer.value)
                {
                    // set the player's depth/z position to be that of the target layer (foreground)
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, foreground.transform.position.z);
                    ChangeLayerOpacity(foreground, 1);
                }
                else if (destinationLayerNum == midgroundLayer.value)
                {
                    // set the player's depth/z position to be that of the target layer (midground)
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, midground.transform.position.z);
                    // fade the foreground so the the midground is more visible
                    ChangeLayerOpacity(foreground, (float)0.8);
                }
                /*
                 * Set the player's own layer to the desired layer integer so that the player will
                 * collide with that layer (self-collision is setup in the layer properites). Calls
                 * SetLayerRecursively so that all children of player will also take the same layer.
                 * This is necessary for changing the interaction trigger, joints, etc.
                 */
                Util.SetLayerRecursively(player, destinationLayerNum);
                // set everything straight after switching layers.
                currentPlayerLayer = destinationLayerNum;

            }
        }
    }


    // Use this for initialization
    void Start()  {
        foregroundLayer = LayerMask.NameToLayer("Foreground");
        midgroundLayer = LayerMask.NameToLayer("Midground");
        currentPlayerLayer = foregroundLayer.value;
        print(midgroundLayer.value);
        print(foregroundLayer.value);
    }

    void Update()
    {
        if (collisionPlayerFeedbackHappening) {
            // Moves the player back to the z depth of the current layer to give the "jerk" effect;s
            player.transform.Translate(new Vector3 (0,0, collisionPlayerFeedbackStartZPos - player.transform.position.z) * Time.deltaTime * 5);
            if (collisionPlayerFeedbackStartZPos == player.transform.position.z || (Mathf.Abs(collisionPlayerFeedbackStartZPos - player.transform.position.z) > 1)) collisionPlayerFeedbackHappening = false;
        }

        // toggle the player layer when the Switch Layer Key is pressed. See layer (not sorting layer) list
        if (Input.GetButtonDown("Switch Layer")) {

           if (currentPlayerLayer == foregroundLayer.value) {
                SwitchPlayerLayer(midgroundLayer.value);
           } else {
                SwitchPlayerLayer(foregroundLayer.value);
            }
        }
    }


    void ChangeLayerOpacity(GameObject layerRoot, float opaqueness) {
        // Get all tilemap renderer components in all the children of the layerRoot gameObject
        TilemapRenderer[] tilemapRenderers = layerRoot.GetComponentsInChildren<TilemapRenderer>();
        // for every tilemapRender component, set its material's "tint color" to be transparent (specified by the opaqueness arguement)
        for (int i = 0, tilemapRenderersLength = tilemapRenderers.Length; i < tilemapRenderersLength; i++) {
            TilemapRenderer mapRenderer = tilemapRenderers[i];
            mapRenderer.material.color = new Color(mapRenderer.material.color.r, mapRenderer.material.color.g, mapRenderer.material.color.b, opaqueness);
        }
    }
}
