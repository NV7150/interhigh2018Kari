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
	
	/// <summary>
	/// エイムを行う最大近接距離
	/// </summary>
	public float aimCapDistance = 0f;
	
	/// <summary>
	/// aimcapに達しているか
	/// </summary>
	private bool isAimCap = false;

	public bool IsAimCap {
		get { return isAimCap; }
	}
	
	/// <summary>
	/// aimCapに達している時、実際のshootFromから物体への距離が格納される
	/// </summary>
	private float realAimDistanceToObject = 0f;

	public float RealAimDistanceToObject {
		get { return realAimDistanceToObject; }
	}

	private int bgLayerMask = 0;
	
	/// <summary>
	/// カメラリコイラー
	/// </summary>
	private CameraRecoiler recoiler;

	public float aimMomentryForcusDegree = 0.2f;

	private float currentAimCorrection = 1.0f;

	public float CurrentAimCorrection {
		get { return currentAimCorrection; }
	}

	// Use this for initialization
	void Start () {
		ik = GetComponent<AimIK>();
		rockDistance = shootRange;
		stateMan = GetComponent<PlayerStateManager>();
		aimForcusSys = GetComponent<AimForcusSystem>();
		recoiler = cam.GetComponentInChildren<CameraRecoiler>();
		
		bgLayerMask = LayerMask.GetMask("BackGround");
	}
	
	// Update is called once per frame
	void Update () {
		
		searchAim();

		bool atk = Input.GetButton("Fire1");
		if (atk) {
			shoot();
			//反動
			recoiler.recoilCemera(recoil);
		} else {
			stateMan.IsShooting = false;
		}
		forcusAim();
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
			var aimpoint = hit.point;
			var dist = Vector3.Distance(shootFrom.transform.position, aimpoint);
			
			if(dist <= aimCapDistance) {
				realAimDistanceToObject = dist;
				aimpoint += cam.transform.forward * aimCapDistance;
				dist = aimCapDistance;
				isAimCap = true;
			} else {
				isAimCap = false;
			}
			//物体に当たればそっちを向く
			ik.solver.IKPosition = aimpoint;
			//距離を更新
			rockDistance = dist;
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

	void forcusAim() {
		
		//エイム動作
		stateMan.IsAiming = Input.GetButton("Aim");
		
		currentAimCorrection = (stateMan.IsAiming) ? aimForcusSys.CurrentForcusRate - aimMomentryForcusDegree : 1.0f;
		currentAimCorrection = (currentAimCorrection > 0) ? currentAimCorrection : 0f;
	}
	
	/// <summary>
	/// 射撃します
	/// </summary>
	void shoot() {
		stateMan.IsShooting = true;
		
		//射撃の方向を取得
		var vector = getBulletVector(currentAimCorrection);
		
		//射撃
		var shootRay = new Ray(shootFrom.position,vector);
		var hit = new RaycastHit();
		if (Physics.Raycast(shootRay,out hit,shootRange,bgLayerMask)) {
			Debug.DrawLine(shootFrom.position,hit.point,Color.blue);
			
			Instantiate(bulletPrefab, hit.point, new Quaternion(0, 0, 0, 0));
		}
		
	}
}
