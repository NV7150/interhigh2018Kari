using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
	public class PlayerHealth : MonoBehaviour {
		/// <summary>
		/// 現在HP
		/// 単位は割合で、0~1
		/// </summary>
		private float hp = 1.0f;

		/// <summary>
		/// 自然回復開始までの残り時間
		/// </summary>
		private float recoveryTimer;

		private PlayerAbilities abilities;
		
		
		public float Hp {
			get { return hp; }
		}

		private void Start() {
			abilities = GetComponent<PlayerAbilities>();
		}

		// Update is called once per frame
		void Update() {
			recovery();
		}

		/// <summary>
		/// 自然回復します
		/// </summary>
		void recovery() {
			if (recoveryTimer < 0f) {
				hp += abilities.RecoveryRate * Time.deltaTime;
				hp = (hp < 1.0f) ? hp : 1.0f;
			} else {
				recoveryTimer -= Time.deltaTime;
			}
		}

		/// <summary>
		/// ダメージを受けます
		/// </summary>
		/// <param name="damage">受けるダメージ値</param>
		public void damage(float damage) {
			damage *= abilities.Protect;
			hp -= damage;
			hp = (hp < 0) ? 0 : hp;
			if (hp < 0) {
				dead();
			}

			recoveryTimer = abilities.RecoveryStart;
		}

		/// <summary>
		/// 死にます
		/// </summary>
		void dead() {
			Debug.Log("dead!");
		}
	}
}