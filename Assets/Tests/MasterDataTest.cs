using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class MasterDataTest : MonoBehaviour {
	public EquipmentManager equipMan;
	
	// Use this for initialization
	void Awake () {
		Weapon weapon = MeleeWeaponMasterManager.INSTANCE.creatWeapon(0);
		equipMan.equip(weapon);
	}

	private void Start() {
		var enemy = EnemyMasterDataManager.INSTANCE.getEnemy(0);
		enemy.transform.position = new Vector3(10,0,-30);
	}

	// Update is called once per frame
	void Update () {
	}
}
