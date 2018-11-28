using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component that emits lasers.
public class LaserEmitter : MonoBehaviour {

	[Tooltip("The object to be instantiated as a laser.")]
	public GameObject laser;
	[Tooltip("The maximum number of reflections/refractions for the laser.")]
	public const int maxReflections = 4;

	private GameObject[] lasers;

	// Use this for initialization
	void Start () {
		lasers = new GameObject[maxReflections];
		for(int i = 0; i < lasers.Length; i ++)
			lasers[i] = GameObject.Instantiate(laser);
	}
	
	// Sets the laser objects up so they appear to be bouncing around.
	void Update () {
		// The origin and direction of the ray.
		Vector2 o = transform.position, d = transform.right;
		
		// Whether or not each ray should be visible.
		bool visible = true;

		foreach(GameObject laser in lasers) {
			// If the laser object should be visible (the laser is "still going")
			if(visible) {
				// Make it visible
				laser.SetActive(true);
				// Find the intersection between the ray and the world.
				RaycastHit2D hit = Physics2D.Raycast(o, d);

				// Make the laser object face the direction of the ray.
				laser.transform.position = (o + hit.point) * 0.5f;
				Vector3 localScale = laser.transform.localScale;
				Vector3 scale = new Vector3(
					Vector2.Distance(o, hit.point),
					localScale.y,
					localScale.z);
				float angle = Vector2.SignedAngle(laser.transform.right, d);
				laser.transform.Rotate(Vector3.forward, angle);
				laser.transform.localScale = scale;

				// Determine whether a laser redirection is available in the hit object. (Reflection or refraction)
				LaserAffector affector = hit.collider.gameObject.GetComponent<LaserAffector>();
				// If so, redirect the laser.
				if(affector != null) {
					Ray2D newRay = affector.RedirectLaser(new Ray2D(o, d), hit);
					o = newRay.origin;
					d = newRay.direction;
				}
				// Otherwise, set the following laser objects to invisible.
				else
					visible = false;
			}
			else 
				laser.SetActive(false);
		}
	}
}
