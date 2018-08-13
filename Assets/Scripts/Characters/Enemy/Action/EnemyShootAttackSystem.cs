using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using RootMotion.FinalIK;
using UnityEngine;

public class EnemyShootAttackSystem : ShootingSystem{
	
	/// <summary>
	/// ステートマネージャ
	/// </summary>
	private EnemyStateManager stateMan;
	
	/// <summary>
	/// AimIkのコンポーネント
	/// </summary>
	private AimIK aimIk;

	private EnemyAbilities abilities;
	private EnemyEquipmentManager equipMan;
	
	
	/// <summary>
	/// 射撃が無視するオブジェクトのマスク
	/// あくまでこれの否定のマスクが射撃を無視する
	/// </summary>
	public LayerMask ignoreMask;

	

	public override float ShootWide {
		get { return abilities.ShootWide; }
	}
	
	public override float ShootRange {
		get { return equipMan.CurrentShootWeapon.Range; }
	}

	protected override float BurstTime {
		get { return equipMan.CurrentShootWeapon.FireSec; }
	}

	protected override float ReloadTime {
		get { return equipMan.CurrentShootWeapon.ReloadSec; }
	}

	protected override float ReloadRate {
		get { return abilities.ReloadRate; }
	}

	protected override int maxAmmo {
		get { return equipMan.CurrentShootWeapon.Ammo; }
	}


	// Use this for initialization
	void Awake () {
		stateMan = GetComponent<EnemyStateManager>();
		aimIk = GetComponent<AimIK>();
		equipMan = GetComponent<EnemyEquipmentManager>();
		abilities = GetComponent<EnemyAbilities>();
		
		//条件を否定
		ignoreMask = ~ignoreMask;
	}
	
	// Update is called once per frame
	void Update () {
		if (stateMan.State == EnemyState.FOUND || stateMan.State == EnemyState.ATTACKING) {
			//見つかってるならエイム対象に銃を向ける
			searchAim();
			
			if (stateMan.State == EnemyState.ATTACKING) {
				//攻撃できる状況にあってなおかつ攻撃できるなら攻撃
				if(updateCanShoot())
					shoot();
			}
		} else {
			normalAim();
		}
	}

	/// <summary>
	/// 見つけてない時のエイム
	/// </summary>
	void normalAim() {
		aimIk.solver.IKPosition = transform.forward * ShootRange + shootFrom.position;
	}
	
	/// <summary>
	/// プレイヤーを見つけてる時のエイム処理
	/// </summary>
	void searchAim() {
		//まずプレイヤーにむける
		aimIk.solver.IKPosition = stateMan.Player.position;
		
		//プレイヤーによりすぎると下をエイムするのであくまで最大射程の部分を狙う：ただしステージは２次元的じゃないと効果がない
		aimIk.solver.IKPosition.x +=
			shootFrom.forward.x * (ShootRange - Vector3.Distance(shootFrom.position, stateMan.Player.position));
		aimIk.solver.IKPosition.z +=
			shootFrom.forward.z * (ShootRange - Vector3.Distance(shootFrom.position, stateMan.Player.position));
		aimIk.solver.IKPosition.y += 1.5f;
	}

	/// <summary>
	/// 射撃します
	/// </summary>
	void shoot() {
		//ベクトル取得してレイを発射
		var vector = getBulletVector(1.0f);
		
		var ray = new Ray(shootFrom.position,vector);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit,ShootRange,ignoreMask)) {
			Debug.DrawLine(shootFrom.position,hit.point,Color.green);
			if (hit.collider.CompareTag("Player")) {
				
			}
		}
		
		//残弾とかの処理
		updateBurstTime();
	}
}
