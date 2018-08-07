using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player {
	public class PlayerShootingSystem : ShootingSystem {

		/// <summary>
		/// エイムを行う最大近接距離
		/// </summary>
		public float aimCapDistance = 0f;

		/// <summary>
		/// エイム開始時に瞬時にズームする値
		/// </summary>
		public float aimMomentryForcusDegree = 0.2f;

		/// <summary>
		/// プレイヤーの能力値
		/// </summary>
		private PlayerAbilities abilities;
		
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

		private int remainingAmmo = 0;


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
		
		private ShootWeapon weapon;
		

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
			get { return abilities.ShootWide; }
		}

		public override float ShootRange {
			get { return weapon.Range; }
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

		public ShootWeapon Weapon {
			get { return weapon; }
		}

		// Use this for initialization
		void Start() {
			weapon = ShootWeaponMasterManager.INSTANCE.creatWeapon(0).GetComponent<ShootWeapon>();
			
			abilities = GetComponent<PlayerAbilities>();
			ik = GetComponent<AimIK>();
			rockDistance = weapon.Range;
			stateMan = GetComponent<PlayerStateManager>();
			aimForcusSys = GetComponent<AimForcusSystem>();

			remainingAmmo = weapon.Ammo;

			bgLayerMask = LayerMask.GetMask("BackGround");
			rockAbleLayerMask = ~(1 << 9);
		}

		// Update is called once per frame
		void Update() {

			searchAim();
			if (burstTimer <= 0f) {
				
				bool atk = (weapon.IsAutomatic) ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
				if (atk) {
					if (remainingAmmo > 0) {
						shoot();
						recoilMan.recoil(weapon.Recoil);
						burstTimer = weapon.FireSec;
					} else {
						reload();
					}
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
			if (Physics.Raycast(ray, out hit, weapon.Range * 10) && !hit.collider.CompareTag("Player")) {
				var aimpoint = hit.point;
				var dist = Vector3.Distance(shootFrom.transform.position, aimpoint);
				//当たったところが射程圏内ならそこをロック
				if (dist < weapon.Range) {
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
					rockDistance = Vector3.Distance(cam.transform.position, aimpoint);

					//物体に当たればそっちを向く
					ik.solver.IKPosition = aimpoint;
				}
			} else {
				ik.solver.IKPosition = ray.direction * weapon.Range + cam.transform.position;
				//距離は最大距離
				rockDistance = Vector3.Distance(cam.transform.position, ik.solver.IKPosition);
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
			var shootRay = new Ray(shootFrom.position, vector);
			var hit = new RaycastHit();
			if (Physics.Raycast(shootRay, out hit, weapon.Range, bgLayerMask)) {
				Debug.DrawLine(shootFrom.position, hit.point, Color.blue);

				Instantiate(bulletPrefab, hit.point, new Quaternion(0, 0, 0, 0));
			}
			
			//残弾を減らす
			remainingAmmo -= 1;
			
			//反動処理
			recoilMan.recoil(weapon.Recoil);
		}
		
		/// <summary>
		/// リロードします
		/// </summary>
		void reload() {
			//リロード時間を計算
			burstTimer = abilities.ReloadRate * weapon.ReloadSec;
			//全弾リロード
			remainingAmmo = weapon.Ammo;
		}
	}
}