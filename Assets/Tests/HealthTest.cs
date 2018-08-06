using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class HealthTest : MonoBehaviour {
	public PlayerHealth health ;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H)) {
			health.damage(0.1f);
		}
		Debug.Log("HP" + health.Hp);
	}
}
