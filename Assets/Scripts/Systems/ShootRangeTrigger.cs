using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRangeTrigger : MonoBehaviour {
	public PlayerShootingSystem shootSys;
	public TPVCamera tpvCam;

	private SphereCollider sphereCol;
	// Use this for initialization
	void Start () {
		sphereCol = GetComponent<SphereCollider>();
		sphereCol.radius = (shootSys.shootRange > tpvCam.DistanceToPlayerM) ? shootSys.shootRange : tpvCam.DistanceToPlayerM;
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
