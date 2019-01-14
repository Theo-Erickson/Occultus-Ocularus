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
    private BoxCollider2D playerCollider;
    private int currentPlayerLayer = 17;
    private int desiredPlayerLayer = 17;
    private LayerMask overlapLayerMask;
    private Collider2D[] detectedOverlaps;

    //call this function to switch the player to a layer (foreground, background, etc...)
    public void SwitchPlayerLayer(int destinationLayerNum)
    {
        desiredPlayerLayer = destinationLayerNum;
        overlapLayerMask = LayerMask.GetMask(LayerMask.LayerToName(desiredPlayerLayer));
        // detects when the player will overlap/ collide with the layer they are attempting to switch to and returns (exits function without doing anything);
        if (Physics2D.OverlapCircle(player.transform.position, (float)0.1, overlapLayerMask) != null) return;
        if (desiredPlayerLayer == 17)
        {
            // set the player's depth/z position to be that of the target layer (foreground)
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, foreground.transform.position.z);
            ChangeLayerOpacity(foreground, 1);
        }
        else if (desiredPlayerLayer == 16)
        {
            // set the player's depth/z position to be that of the target layer (midground)
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, midground.transform.position.z);
            // fade the foreground so the the midground is more visible
            ChangeLayerOpacity(foreground, (float)0.8);
        }
        /*
         * Set the player's own layer to the desired layer integer so that the player will
         * collide with that layer (self-collision is settup in the layer properites). Calls
         * SetLayerRecursively so that all children of player will also take the same layer.
         * This is necessary for changing the interaction trigger, joints, etc.
         */
        SetLayerRecursively(player, desiredPlayerLayer);
        // set everything straight after switching layers.
        currentPlayerLayer = desiredPlayerLayer;
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
    
        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (currentPlayerLayer != desiredPlayerLayer) {

        }
        // Todo: make this key intependent
        // toggle the player layer when L is pressed. (layers 16 & 17 correspond to foreground and background in the layer list)
        if (Input.GetKeyDown(KeyCode.L)) {
           if (desiredPlayerLayer == 17) {
                SwitchPlayerLayer(16);
           } else {
                SwitchPlayerLayer(17);
            }
        }
    }


    void ChangeLayerOpacity(GameObject layerRoot, float opaqueness) {
        // Get all tilemap renderer components in all the children of the layerRoot gameObject
        TilemapRenderer[] tilemapRenderers = layerRoot.GetComponentsInChildren<TilemapRenderer>();
        // for every tilemapRender component, set its material's "tint color" to be transparent (specifie by the opaqueness arguement)
        for (int i = 0, tilemapRenderersLength = tilemapRenderers.Length; i < tilemapRenderersLength; i++) {
            TilemapRenderer mapRenderer = tilemapRenderers[i];
            mapRenderer.material.color = new Color(mapRenderer.material.color.r, mapRenderer.material.color.g, mapRenderer.material.color.b, opaqueness);
        }
    }
}
