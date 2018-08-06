using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
    public class PlayerAbilities : MonoBehaviour {
		/// <summary>
		/// 筋力：リロード速度、反動制御、近接攻撃力に関係
		/// 一部武器の装備条件
		/// </summary>
		private int strength;
		
		/// <summary>
		/// 技巧：命中円の大きさと収縮速度、エイム時の足の早さ、足音の消音性に関係
		/// </summary>
		private int technic;
		
		/// <summary>
		/// 敏捷性：足の早さ、気づかれる確率、隠密時の足の早さ、近接武器の振りの速さに関係
		/// </summary>
		private int agility;
		
		/// <summary>
		/// 体力：HP、重量に関係
		/// </summary>
		private int toughness;
		
		
		/// <summary>
		/// リロード速度
		/// 単位は割合：1.0f~0.5f
		/// 計算式：-0.01f * strength + 1.00f
		/// </summary>
		private float reloadRate;
		
		/// <summary>
		/// 反動制御
		/// 単位は不明(とにかく1000f~5000f)
		/// 計算式：80f * strength + 1000f
		/// </summary>
		private float recoilControll;
		
		/// <summary>
		/// 近接攻撃力
		/// </summary>
		private float meleeAttack;
		
		/// <summary>
		/// 命中円半径
		/// 単位は不明(とにかく10f~2f)
		/// 計算式：-0.16f * technic + 10.0f
		/// </summary>
		private float shootWide;
		
		/// <summary>
		/// エイム速度(命中円の狭まる速さ)
		/// 単位は割合/秒(0.5f~8f)
		/// 計算式：0.15f * technic + 0.5f
		/// </summary>
		private float forcusSpeed;
		
		/// <summary>
		/// 感知範囲の大きさ
		/// 単位はunity単位の長さ(10~2.5)
		/// 計算式：-0.15f * technic + 10f
		/// </summary>
		private float sneakRad;
	    
	    /// <summary>
	    /// エイム時の足の早さの割合
	    /// 単位は割合(0.3f~0.8f)
	    /// 計算式：0.01f * technic + 0.3f
	    /// </summary>
	    private float aimingSpeedRate;
		
		/// <summary>
		/// 移動速度
		/// 250f~750f
		/// 計算式：10f * agility + 250f
		/// </summary>
		private float walkSpeed;
		
		//気づかれる確率はagiそのままを使用
		
		/// <summary>
		/// 近接武器の振りの速さ
		/// 単位は割合(1.5f~0.5f)
		/// 計算式：-0.02f * agility + 1.5f
		/// </summary>
		private float meleeAttackSpeedRate;
	    
	    /// <summary>
	    /// 隠密時の足の早さの割合
	    /// 単位は割合(0.3f~0.8f)
	    /// 計算式：0.01f * agility + 0.3f
	    /// </summary>
	    private float sneakSpeedRate;
	    
	    public int Strength {
			get { return strength; }
		}
	
		public int Technic {
			get { return technic; }
		}
	
		public int Agility {
			get { return agility; }
		}
	
		public int Toughness {
			get { return toughness; }
		}
	
		public float ReloadRate {
			get { return reloadRate; }
		}
	
		public float RecoilControll {
			get { return recoilControll; }
		}
	
		public float MeleeAttack {
			get { return meleeAttack; }
		}
	
		public float ShootWide {
			get { return shootWide; }
		}
	
		public float ForcusSpeed {
			get { return forcusSpeed; }
		}
	    
	    public float AimingSpeedRate {
		    get { return aimingSpeedRate; }
	    }
	
		public float SneakRad {
			get { return sneakRad; }
		}
	
		public float WalkSpeed {
			get { return walkSpeed; }
		}
	
		public float MeleeAttackSpeedRate {
			get { return meleeAttackSpeedRate; }
		}
	    
	    public float SneakSpeedRate {
		    get { return sneakSpeedRate; }
	    }

	    private void Start() {
		    defAbilities();
	    }

	    /// <summary>
	    /// 各能力値を計算します
	    /// </summary>
	    public void defAbilities() {
		    reloadRate = -0.01f * strength + 1.00f;
		    recoilControll = 80 * strength + 1000f;
		    //ここに近接攻撃力の計算式
		    shootWide = -0.16f * technic + 10.0f;
		    forcusSpeed = 0.15f * technic + 0.5f;
		    sneakRad = -0.15f * technic + 10f;
		    aimingSpeedRate = 0.01f * technic + 0.3f;
		    walkSpeed = 10f * agility + 250f;
		    meleeAttackSpeedRate = -0.02f * agility + 1.5f;
		    sneakSpeedRate = 0.01f * agility + 0.3f;
	    }
		
		/// <summary>
		/// 筋力を１増加させます
		/// </summary>
		public void plusStr() {
			strength += (strength < 50) ? 1 : 0;
			
			reloadRate = -0.01f * strength + 1.00f;
			recoilControll = 80 * strength + 1000f;
			//ここに近接攻撃力の計算式
		}
		
		/// <summary>
		/// 技巧を１増加させます
		/// </summary>
		public void plusTech() {
			technic += (technic < 50) ? 1 : 0;
			shootWide = -0.16f * technic + 10.0f;
			forcusSpeed = 0.15f * technic + 0.5f;
			sneakRad = -0.15f * technic + 10f;
			aimingSpeedRate = 0.01f * technic + 0.3f;
		}
		
		/// <summary>
		/// 敏捷性を１増加させます
		/// </summary>
		public void plusAgi() {
			agility += (agility < 50) ? 1 : 0;
			walkSpeed = 10f * agility + 250f;
			meleeAttackSpeedRate = -0.02f * agility + 1.5f;
			sneakSpeedRate = 0.01f * agility + 0.3f;
		}
	}
}
