using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Affects how a laser is refracted or reflected on a surface.
public abstract class LaserAffector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// A function that will redirect the laser when the object is hit.
	public abstract Ray2D RedirectLaser(Ray2D inRay, RaycastHit2D hit);
}
