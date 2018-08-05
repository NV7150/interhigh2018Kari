using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerShootingSystem : ShootingSystem {
	
	/// <summary>
	/// 射撃の振れる幅
	/// 単位は割合（1でVecotor3の長さでの半径1の円一つ分ぶれる）
	/// </summary>
	public float shootWide;
	
	/// <summary>
	/// 射程
	/// 単位はVector3のと同じ
	/// </summary>
	public float shootRange;
	
	/// <summary>
	/// エイムを行う最大近接距離
	/// </summary>
	public float aimCapDistance = 0f;
	
	/// <summary>
	/// リコイルする値
	/// 単位は度/秒の速度
	/// </summary>
	public float recoil;
	
	/// <summary>
	/// エイム開始時に瞬時にズームする値
	/// </summary>
	public float aimMomentryForcusDegree = 0.2f;
	
	/// <summary>
	/// 発砲間隔
	/// </summary>
	public float burstSeconds = 0.1f;
	
	
	
	/// <summary>
	/// ロック中の物体からカメラへの距離
	/// </summary>
	private float rockDistance;
	
	/// <summary>
	/// aimcapに達しているか
	/// </summary>
	private bool isAimCap = false;

	/// <summary>
	/// aimCapに達している時、実際のshootFromから物体への距離が格納される
	/// </summary>
	private float realAimDistanceToObject = 0f;

	/// <summary>
	/// 現在のエイムフォーカス値
	/// 単位は割合
	/// </summary>
	private float currentAimCorrection = 1.0f;
	
	/// <summary>
	/// 発砲間隔を計測する変数
	/// </summary>
	private float burstTimer = 0.0f;
	
	
	
	/// <summary>
	/// AimIK
	/// </summary>
	private AimIK ik;
	
	/// <summary>
	/// ステートマネージャ
	/// </summary>
	private PlayerStateManager stateMan;
	
	/// <summary>
	/// エイムズームを行うコンポーネント
	/// </summary>
	private AimForcusSystem aimForcusSys;
	
	
	
	/// <summary>
	/// TPSカメラのオブジェクト
	/// </summary>
	public Camera cam;
	
	/// <summary>
	/// 銃弾オブジェクトのプレファブ
	/// </summary>
	public GameObject bulletPrefab;
	
	/// <summary>
	/// エイムオブジェクトをリコイルするもの
	/// </summary>
	public AimObjectRecoiler recoilMan;
	
	/// <summary>
	/// カメラから独立してエイムの補助を行うオブジェクト
	/// </summary>
	public AimObject aimObj;
	
	
	
	/// <summary>
	/// 背景データのマスク
	/// </summary>
	private int bgLayerMask = 0;
	
	/// <summary>
	/// unrockable以外のもののマスク
	/// プレイヤーにめっちゃ近いところをロックする不具合が生じているので今は不使用
	/// </summary>
	private int rockAbleLayerMask = 0;
	

	public override float ShootWide {
		get { return shootWide; }
	}
	
	public override float ShootRange {
		get { return shootRange; }
	}
	
	public float RockDistance {
		get { return rockDistance; }
	}
	
	public bool IsAimCap {
		get { return isAimCap; }
	}
	
	public float RealAimDistanceToObject {
		get { return realAimDistanceToObject; }
	}
	
	public float CurrentAimCorrection {
		get { return currentAimCorrection; }
	}
	
	// Use this for initialization
	void Start () {
		ik = GetComponent<AimIK>();
		rockDistance = shootRange;
		stateMan = GetComponent<PlayerStateManager>();
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
				recoilMan.recoil(recoil);
				burstTimer = burstSeconds;
			}
		} else {
			burstTimer -= Time.deltaTime;
			stateMan.IsShooting = false;
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
					isAimCap = true;
				} else {
					isAimCap = false;
				}
					
				//距離を更新：カメラからの距離の方がaimcursor的に都合がいいのでそっち
				rockDistance = Vector3.Distance(cam.transform.position,aimpoint);

				//物体に当たればそっちを向く
				ik.solver.IKPosition = aimpoint;
			}
		} else {
			ik.solver.IKPosition = ray.direction * shootRange + cam.transform.position;
			//距離は最大距離
			rockDistance = Vector3.Distance(cam.transform.position,ik.solver.IKPosition);
		}
//		cheakIKPosLegal();
	}
	
	/// <summary>
	/// エイムズーム値の計算
	/// </summary>
	void forcusAim() {
		//入力管理
		if (Input.GetButtonDown("Aim")) {
			stateMan.IsAiming = true;
		}

		if (Input.GetButtonUp("Aim")) {
			stateMan.IsAiming = false;
		}
		
		//エイムフォーカスシステムの値を通じて補正値を設定
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
		
		//反動処理
		recoilMan.recoil(recoil);
	}

	void cheakIKPosLegal() {
		if(Mathf.DeltaAngle(transform.position.y,ik.solver.IKPosition.y) > 90) {
			ik.solver.IKPosition.y = transform.position.y;
			ik.solver.IKPosition += transform.forward * shootRange;
		}
		var xzPos = new Vector2(ik.solver.IKPosition.x,ik.solver.IKPosition.z).normalized;
		if (Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), xzPos) < 100) {
			ik.solver.IKPosition *= -1;
		}
		
	}
}
