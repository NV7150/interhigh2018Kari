using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class IkTest : MonoBehaviour {
	private AimIK ik;
	
	// Use this for initialization
	void Start () {
		ik = GetComponent<AimIK>();
	}
	
	// Update is called once per frame
	void Update () {
		var vector = transform.rotation * new Vector3(0,0,10);
		vector += transform.position;
		ik.solver.IKPosition = vector;
	}
}
