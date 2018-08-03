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
	private PlayerStateManager _stateMan;
	
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

	private int rockAbleLayerMask = 0;

	public float aimMomentryForcusDegree = 0.2f;

	private float currentAimCorrection = 1.0f;

	public float CurrentAimCorrection {
		get { return currentAimCorrection; }
	}

	public float burstSeconds = 0.1f;

	private float burstTimer = 0.0f;

	public PlayerRecoilManager recoilMan;

	public AimObject aimObj;

//	private PlayerInputManager inputMan;

	// Use this for initialization
	void Start () {
		ik = GetComponent<AimIK>();
		rockDistance = shootRange;
		_stateMan = GetComponent<PlayerStateManager>();
		aimForcusSys = GetComponent<AimForcusSystem>();
		
		bgLayerMask = LayerMask.GetMask("BackGround");
		rockAbleLayerMask = ~(1 << 9);
	}
	
	// Update is called once per frame
	void Update () {
		
		searchAim();
		if (burstTimer <= 0f) {
			bool atk = Input.GetButtonDown("Fire1");
			if (atk) {
				shoot();
				//反動
//				recoiler.recoilCemera(recoil);
				recoilMan.recoil(recoil);
				burstTimer = burstSeconds;
			}
		} else {
			burstTimer -= Time.deltaTime;
			_stateMan.IsShooting = false;
		}

		forcusAim();
	}
	
	/// <summary>
	/// 銃口の向き（AimIKの位置）を修正します。
	/// update毎に呼ばれます。
	/// </summary>
	void searchAim() {
		//エイムオブジェクトからRayを発信
		var ray = aimObj.getFrontRay();
		
		RaycastHit hit;
		//とりあえずPlayer以外なら命中判定
		if (Physics.Raycast(ray, out hit,shootRange * 10) && !hit.collider.CompareTag("Player")) {
			var aimpoint = hit.point;
			var dist = Vector3.Distance(shootFrom.transform.position, aimpoint);
			//当たったところが射程圏内ならそこをロック
			if (dist < shootRange) {
				//エイムキャップ圏内なら壁にめりこむ
				if (dist <= aimCapDistance) {
					//実距離を保存
					realAimDistanceToObject = dist;
					//aimCapになるまでaimpointを調節
					aimpoint += cam.transform.forward * (aimCapDistance - realAimDistanceToObject);
					dist = aimCapDistance;
					isAimCap = true;
				} else {
					isAimCap = false;
				}

				//物体に当たればそっちを向く
				ik.solver.IKPosition = aimpoint;
				//距離を更新
				rockDistance = Vector3.Distance(cam.transform.position,hit.point);
			}
		} else {
			ik.solver.IKPosition = ray.direction * shootRange + cam.transform.position;
			//距離は最大距離
			rockDistance = Vector3.Distance(cam.transform.position,ik.solver.IKPosition);
		}
	}

	void forcusAim() {
		if (Input.GetButtonDown("Aim")) {
			_stateMan.IsAiming = true;
		}

		if (Input.GetButtonUp("Aim")) {
			_stateMan.IsAiming = false;
		}

		currentAimCorrection = (_stateMan.IsAiming) ? aimForcusSys.CurrentForcusRate - aimMomentryForcusDegree : 1.0f;
		currentAimCorrection = (currentAimCorrection > 0) ? currentAimCorrection : 0f;
	}
	
	/// <summary>
	/// 射撃します
	/// </summary>
	void shoot() {
		_stateMan.IsShooting = true;
		
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
