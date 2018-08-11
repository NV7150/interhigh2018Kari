using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player {
	/// <summary>
	/// 近接武器と射撃武器の切り替えを行います
	/// </summary>
    public class PlayerEquipmentSwitcher : MonoBehaviour {
	    /// <summary>
	    /// エイムカーソルのオブジェクト
	    /// </summary>
	    public GameObject aimCursor;
	    
		//近接射撃関わらず変動するコンポーネント
	    private PlayerStateManager stateMan;
	    
	    private Animator animator;
		
		//ここから射撃系のプレイヤーコンポーネント　注：AimCursorもそう(publicなオブジェクトだから上にあるだけ)
	    private AimIK aimIk;

		private PlayerAimer aimer;
		
		private PlayerShootingSystem shootSys;

		private AimForcusSystem forcusSys;
		
		//ここから近接系のプレイヤーコンポーネント
		private PlayerMeleeSystem meleeSys;
	    
        // Use this for initialization
        void Awake () {
	        shootSys = GetComponent<PlayerShootingSystem>();
	        meleeSys = GetComponent<PlayerMeleeSystem>();
	        animator = GetComponent<Animator>();
	        aimIk = GetComponent<AimIK>();
	        stateMan = GetComponent<PlayerStateManager>();
	        aimer = GetComponent<PlayerAimer>();
	        forcusSys = GetComponent<AimForcusSystem>();
        }
		
	    
	    /// <summary>
	    /// 各スクリプトの状態を射撃に変更します
	    /// </summary>
	    public void switchShoot(Transform shootAim,Transform shootFrom) {
			    //aimIKの対象を射撃武器に変更
			aimIk.enabled = true;
			aimIk.solver.axis = new Vector3(0, 0, 1);
			aimIk.solver.transform = shootAim;
			aimIk.solver.target = null;

			//各武器システムの有効化
			meleeSys.enabled = false;
			shootSys.enabled = true;
			aimer.enabled = true;
			forcusSys.enabled = true;

			//各武器システムの設定
			shootSys.shootFrom = shootFrom;
			aimer.shootFrom = shootFrom;

			//アニメータのレイヤのweight設定
			animator.SetLayerWeight(2, 0);
			animator.SetLayerWeight(1, 1);

			//エイムカーソルの有効化
			aimCursor.SetActive(true);
	    }
	    
		
	    /// <summary>
	    /// 各スクリプトの状態を近接に変更します
	    /// </summary>
	    public void switchMelee() {
		    //aimIKを無効化
		    aimIk.enabled = false;
		    
		    //各武器システムの有効化
		    meleeSys.enabled = true;
		    shootSys.enabled = false;
		    aimer.enabled = false;
		    forcusSys.enabled = false;
		    
		    //アニメータのレイヤのweight設定
		    animator.SetLayerWeight(2,1);
		    animator.SetLayerWeight(1,0);
		    
		    //エイムカーソルの無効化
		    aimCursor.SetActive(false);
		    
		    //射撃フラグの解消
		    stateMan.IsShooting = false;
	    }
    }
}
