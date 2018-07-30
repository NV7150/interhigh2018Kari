using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerShootingSystem : ShootingSystem {
	
	/// <summary>
	/// 射撃の触れる幅
	/// 単位は割合（1でQuatanion1の範囲でブレる）
	/// </summary>
	public float shootWide;
	
	public override float ShootWide {
		get { return shootWide; }
	}
	
	/// <summary>
	/// 射程
	/// 単位はVector3のと同じ
	/// </summary>
	public float shootRange;
	
	public override float ShootRange {
		get { return shootRange; }
	}
	
	/// <summary>
	/// 銃弾オブジェクトのプレファブ
	/// </summary>
	public GameObject bulletPrefab;
	
	/// <summary>
	/// TPSカメラのオブジェクト
	/// </summary>
	public Camera cam;

	private TPVCamera tpvCam;
	
	/// <summary>
	/// AimIK
	/// </summary>
	private AimIK ik;
	
	/// <summary>
	/// ロック中の物体への距離
	/// </summary>
	private float rockDistance;
	
	public float RockDistance {
		get { return rockDistance; }
	}
	
	/// <summary>
	/// リコイルする値
	/// 単位は度/秒の最高速度
	/// 加速度はこれの1/10
	/// </summary>
	public float recoil;
	
	/// <summary>
	/// ステートマネージャ
	/// </summary>
	private PlayerStateManager stateMan;
	
	private AimForcusSystem aimForcusSys;

	// Use this for initialization
	void Start () {
		ik = GetComponent<AimIK>();
		rockDistance = shootRange;
		tpvCam = cam.GetComponent<TPVCamera>();
		stateMan = GetComponent<PlayerStateManager>();
		aimForcusSys = GetComponent<AimForcusSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
		searchAim();

		bool atk = Input.GetButton("Fire1");
		if (atk) {
			shoot();
			//反動
			tpvCam.recoilCemera(recoil);
		} else {
			stateMan.IsShooting = false;
		}
	}
	
	/// <summary>
	/// 銃口の向き（AimIKの位置）を修正します。
	/// update毎に呼ばれます。
	/// </summary>
	void searchAim() {
		//カメラからRayを発信
		var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		RaycastHit hit;
		//とりあえずPlayer以外なら命中判定
		if (Physics.Raycast(ray, out hit,shootRange) && !hit.collider.gameObject.CompareTag("Player")) {
			//物体に当たればそっちを向く
			ik.solver.IKPosition = hit.point;
			//距離を更新
			rockDistance = Vector3.Distance(transform.position, hit.point);
		} else {
			//Rayの終端を計算//
			//元となるベクトル（射程）
			var vector = new Vector3(0, 0,shootRange);
			//射程をカメラの回転度分回転
			vector = cam.transform.rotation * vector;
			//カメラのpositionを加算
			vector = cam.transform.position + vector;
			//Rayの終端を向く
			ik.solver.IKPosition = vector;
			//距離は最大距離
			rockDistance = shootRange;
		}
	}

	/// <summary>
	/// 射撃します
	/// </summary>
	void shoot() {
		stateMan.IsShooting = true;
		//射撃の方向を取得
		var vector = getBulletVector(aimForcusSys.CurrentForcusRate);
		
		//射撃
		var shootRay = new Ray(shootFrom.position,vector);
		var hit = new RaycastHit();
		if (Physics.Raycast(shootRay,out hit,shootRange)) {
			Debug.DrawLine(shootFrom.position,hit.point,Color.blue);
			
			Instantiate(bulletPrefab, hit.point, new Quaternion(0, 0, 0, 0));
		}
		
	}
}
