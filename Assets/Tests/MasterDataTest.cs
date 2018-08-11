using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class MasterDataTest : MonoBehaviour {
	public PlayerEquipmentManager equipMan;
	
	// Use this for initialization
	void Awake () {
		Weapon weapon = ShootWeaponMasterManager.INSTANCE.creatWeapon(1);
		equipMan.equip(weapon);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
