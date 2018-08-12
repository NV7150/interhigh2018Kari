using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player {
	public class PlayerShootingSystem : ShootingSystem {
		/// <summary>
		/// エイム開始時に瞬時にズームする値
		/// </summary>
		public float aimMomentryForcusDegree = 0.2f;
		
		/// <summary>
		/// 現在のエイムフォーカス値
		/// 単位は割合
		/// </summary>
		private float currentAimCorrection = 1.0f;

		
		private PlayerAbilities abilities;
		
		private PlayerStateManager stateMan;

		private AimForcusSystem aimForcusSys;

		private PlayerEquipmentManager equipMan;


		/// <summary>
		/// 銃弾オブジェクトのプレファブ
		/// </summary>
		public GameObject bulletPrefab;

		/// <summary>
		/// エイムオブジェクトをリコイルするスクリプト
		/// </summary>
		public AimObjectRecoiler recoilMan;


		/// <summary>
		/// 背景データのマスク
		/// </summary>
		private int bgLayerMask = 0;


		public override float ShootWide {
			get { return abilities.ShootWide; }
		}

		public override float ShootRange {
			get { return equipMan.CurrentShootWeapon.Range; }
		}

		public float CurrentAimCorrection {
			get { return currentAimCorrection; }
		}

		protected override float BurstTime {
			get { return equipMan.CurrentShootWeapon.FireSec; }
		}

		protected override float ReloadTime {
			get { return equipMan.CurrentShootWeapon.ReloadSec; }
		}

		protected override float ReloadRate {
			get { return equipMan.CurrentShootWeapon.ReloadSec; }
		}

		protected override int maxAmmo {
			get { return equipMan.CurrentShootWeapon.Ammo; }
		}


		private void Awake() {
			abilities = GetComponent<PlayerAbilities>();
			stateMan = GetComponent<PlayerStateManager>();
			aimForcusSys = GetComponent<AimForcusSystem>();
			equipMan = GetComponent<PlayerEquipmentManager>();
			
			bgLayerMask = LayerMask.GetMask("BackGround");
		}

		// Update is called once per frame
		void Update() {
			if (updateCanShoot()) {
				bool atk = (equipMan.CurrentShootWeapon.IsAutomatic) ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
				if (atk) {
					shoot();
					recoilMan.recoil(equipMan.CurrentShootWeapon.Recoil);
				}
			} else {
				stateMan.IsShooting = false;
			}

			forcusAim();
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
			if (Physics.Raycast(shootRay, out hit, equipMan.CurrentShootWeapon.Range, bgLayerMask)) {
				Debug.DrawLine(shootFrom.position, hit.point, Color.blue);

				Instantiate(bulletPrefab, hit.point, new Quaternion(0, 0, 0, 0));
			}
			
			//残弾を減らす
			updateBurstTime();
			
			//反動処理
			recoilMan.recoil(equipMan.CurrentShootWeapon.Recoil);
		}
	}
}