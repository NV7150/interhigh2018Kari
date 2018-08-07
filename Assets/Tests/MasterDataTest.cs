using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataTest : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)) {
			var gun = ShootWeaponMasterManager.INSTANCE.creatWeapon(0);
			var gunPro = gun.GetComponent<ShootWeapon>();
			Debug.Log(gunPro.Name);
		}
	}
}
