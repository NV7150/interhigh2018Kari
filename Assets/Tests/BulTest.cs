using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulTest : MonoBehaviour {
	private bool stop = false;
	private float dist = 0f;

	public int range;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!stop) {
			dist += transform.up.sqrMagnitude;
			if (dist > range)
				stop = true;
			transform.position += transform.up;
		}
	}

	private void OnCollisionEnter(Collision other) {
		stop = true;
	}
}
