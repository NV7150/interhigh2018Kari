using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using Characters.Player;
using UnityEngine;

public class EnemyMeleeSystem : MeleeSystem {

	private EnemyEquipmentManager equipMan;
	private EnemyAbilities abilities;
	private EnemyStateManager stateMan;

	protected override float attackSpeedRate {
		get { return abilities.MeleeAttack; }
	}

	// Use this for initialization
	private void Awake() {
		equipMan = GetComponent<EnemyEquipmentManager>();
		abilities = GetComponent<EnemyAbilities>();
		stateMan = GetComponent<EnemyStateManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (stateMan.State == EnemyState.ATTACKING) {
			attack();
		}
	}

	public override void hit(GameObject hitObj) {
		var plHelth = hitObj.GetComponent<PlayerHealth>();
		plHelth.damage(equipMan.CurrentMeleeWeapon.Damage * abilities.MeleeAttack);
	}
}
