using System;
using UnityEngine;

namespace Characters.Enemy {
	public class EnemyAbilities : Ability {
		/// <summary>
		/// 回転する速さ
		/// 単位は角度/秒（？） 50f~150f
		/// 計算式は(2 * str + 50)
		/// </summary>
		private float rotateSpeed;
		
		/// <summary>
		/// 隠密判定の間隔
		/// 単位は秒 2.0f~0.5f
		/// 計算式は(-0.03f * tech + 2.0)
		/// </summary>
		private float judgeSec;
		
		/// <summary>
		/// 能力値が設定済みかどうか
		/// </summary>
		private bool isBuilderSetted = false;

		public float RotateSpeed {
			get { return rotateSpeed; }
		}

		public float JudgeSec {
			get { return judgeSec; }
		}

		/// <summary>
		/// ビルダーを使って能力値を設定します
		/// </summary>
		/// <param name="builder">能力値が設定されたビルダーオブジェクト</param>
		public void setBuilder(EnemyBuilder builder) {
			//一応のセキュリティ
			if (!isBuilderSetted) {
				Strength = builder.Str;
				Technic = builder.Tech;
				Agility = builder.Agi;
				Toughness = builder.Tough;

				isBuilderSetted = true;
			}
		}

		protected override void defStrParam() {
			base.defTechParam();
			rotateSpeed = 2f * Strength + 50f;
		}

		protected override void defTechParam() {
			base.defTechParam();
			judgeSec = -0.03f * Technic + 2.0f;
		}
	}
}
