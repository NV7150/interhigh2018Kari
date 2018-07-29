using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player {
    public class PlayerEquipmentSystem : MonoBehaviour {
	    /// <summary>
	    /// 射撃動作コンポーネント
	    /// </summary>
	    private PlayerShootingSystem shootSys;
	    /// <summary>
	    /// 近接動作コンポーネント
	    /// </summary>
	    private PlayerMeleeSystem meleeSys;

	    private PlayerStateManager stateMan;
	    
	    /// <summary>
	    /// エイムカーソルのオブジェクト
	    /// </summary>
	    public GameObject aimCursor;
	    
	    /// <summary>
	    /// 射撃武器のオブジェクト
	    /// </summary>
	    public GameObject shootWp;
	    /// <summary>
	    /// 近接武器のオブジェクト
	    /// </summary>
	    public GameObject meleeWp;

	    public Transform shootAim;
	    public Transform meleeAim;
	    
	    /// <summary>
	    /// アニメーター
	    /// </summary>
	    private Animator animator;
	    /// <summary>
	    /// AimIK
	    /// </summary>
	    private AimIK aimIk;
	    
        // Use this for initialization
        void Start () {
	        shootSys = GetComponent<PlayerShootingSystem>();
	        meleeSys = GetComponent<PlayerMeleeSystem>();
	        animator = GetComponent<Animator>();
	        aimIk = GetComponent<AimIK>();
	        stateMan = GetComponent<PlayerStateManager>();
        }
	
        // Update is called once per frame
        void Update () {
	        bool change = Input.GetButtonDown("ChangeWeapon");
	        if (change) {
		        switchWeapon();
	        }
        }

	    private void switchWeapon() {
		    //シューティングシステムが有効であれば
		    if (shootSys.enabled) {
			    switchMelee();
		    } else {
			    switchShoot();
		    }
	    }
	    
	    /// <summary>
	    /// 射撃に切り替え
	    /// </summary>
	    private void switchShoot() {
		    //武器の有効化
		    meleeWp.SetActive(false);
		    shootWp.SetActive(true);
		    //aimIKの対象を射撃武器に変更
		    aimIk.enabled = true;
		    aimIk.solver.axis = new Vector3(1,0,0);
		    aimIk.solver.transform = shootAim.transform;
		    aimIk.solver.target = null;
		    //各武器システムの有効化
		    meleeSys.enabled = false;
		    shootSys.enabled = true;
		    //アニメータのレイヤのweight設定
		    animator.SetLayerWeight(2,0);
		    animator.SetLayerWeight(1,1);
		    //エイムカーソルの有効化
		    aimCursor.SetActive(true);
	    }
	    
	    /// <summary>
	    /// 近接に切り替え
	    /// </summary>
	    private void switchMelee() {
		    //武器の有効化
		    meleeWp.SetActive(true);
		    shootWp.SetActive(false);
		    //aimIKの対象を近接武器に変更
		    aimIk.enabled = false;
		    //各武器システムの有効化
		    meleeSys.enabled = true;
		    shootSys.enabled = false;
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
