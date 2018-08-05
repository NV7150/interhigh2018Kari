﻿using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using RootMotion.FinalIK;
using UnityEngine;

public class EnemyShootAttackSystem : ShootingSystem{
	public float shootWide;
	public override float ShootWide {
		get { return shootWide; }
	}
	
	public float shootRange;
	public override float ShootRange {
		get { return shootRange; }
	}
	
	/// <summary>
	/// ステートマネージャ
	/// </summary>
	private EnemyStateManager stateMan;
	
	/// <summary>
	/// AimIkのコンポーネント
	/// </summary>
	private AimIK aimIk;

	/// <summary>
	/// エイムする速さ
	/// </summary>
	private readonly float AIMING_SPEED = 10.0f;
	
	// Use this for initialization
	void Start () {
		stateMan = GetComponent<EnemyStateManager>();
		aimIk = GetComponent<AimIK>();
	}
	
	// Update is called once per frame
	void Update () {
		if (stateMan.State == EnemyState.FOUND || stateMan.State == EnemyState.ATTACKING) {
			searchAim();
			if(stateMan.State == EnemyState.ATTACKING)
				shoot();
		} else {
			normalAim();
		}
	}

	void normalAim() {
		aimIk.solver.IKPosition = transform.forward * shootRange + shootFrom.position;
	}
	
	void searchAim() {
		aimIk.solver.IKPosition = stateMan.Player.position;
		aimIk.solver.IKPosition.x +=
			shootFrom.forward.x * (shootRange - Vector3.Distance(shootFrom.position, stateMan.Player.position));
		aimIk.solver.IKPosition.z +=
			shootFrom.forward.z * (shootRange - Vector3.Distance(shootFrom.position, stateMan.Player.position));
		aimIk.solver.IKPosition.y += 1.5f;
	}

	void shoot() {
		var vector = getBulletVector(1.0f);
		
		var ray = new Ray(shootFrom.position,vector);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit,shootRange)) {
			Debug.DrawLine(shootFrom.position,hit.point,Color.green);
			if (hit.collider.CompareTag("Player")) {
//				Debug.Log("hit!!!");
			}
		}
	}
}
